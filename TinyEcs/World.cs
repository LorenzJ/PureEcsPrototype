using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

namespace TinyEcs
{
    /// <summary>
    /// The world class.
    /// Can not be inherited from.
    /// Use World.Create() to create an instance.
    /// </summary>
    public sealed partial class World
    {
        private const int initialSize = 2048;
        private int size = initialSize;
        private int highestEntityHandle = 0;
        private Queue<int> openEntityHandles = new Queue<int>();

        private Dictionary<Type, object> resourceMap = new Dictionary<Type, object>();
        private Dictionary<(Type[], Type[]), ComponentGroup> groupMap = new Dictionary<(Type[], Type[]), ComponentGroup>();

        private Dictionary<Type, IComponentContainer> componentContainerMap = new Dictionary<Type, IComponentContainer>();

        private List<ComponentGroup> componentGroups = new List<ComponentGroup>();
        private Dictionary<ISystem, List<GroupInjector>> groupInjectorMap = new Dictionary<ISystem, List<GroupInjector>>();
        private Dictionary<ArcheType, ComponentGroup[]> archeTypeAffectingGroups = new Dictionary<ArcheType, ComponentGroup[]>();
        private Lookup<Type, ISystem> systemMessageMap;
        private int archeTypeId;

        /// <summary>
        /// Create a new Entity handle
        /// </summary>
        /// <returns>Entity handle</returns>
        public Entity CreateEntity()
        {
            // Try to fill open spots
            if (openEntityHandles.Count > 0)
            {
                return new Entity(openEntityHandles.Dequeue());
            }
            else
            {
                // Get a new handle
                var handle = highestEntityHandle++;
                // Resize component containers if required
                if (handle >= size)
                {
                    size *= 2;
                    foreach (var entry in componentContainerMap)
                    {
                        entry.Value.Resize(size);
                    }
                }
                // Return an entity with the new handle
                return new Entity(handle);
            }
        }

        public Entity CreateEntity(ArcheType archeType)
        {
            Entity entity = CreateEntity();
            foreach (var group in archeTypeAffectingGroups[archeType])
            {
                group.Add(entity);
            }
            return entity;
        }

        public ArcheType CreateArcheType(params Type[] types)
        {
            var groupSet = new HashSet<ComponentGroup>();
            foreach (var type in types)
            {
                var groupList = componentGroups.Where(g => g.Contains(type));
                foreach (var group in groupList)
                {
                    groupSet.Add(group);
                }
            }

            var archeType = new ArcheType(archeTypeId++);
            archeTypeAffectingGroups.Add(archeType, groupSet.ToArray());
            return archeType;
        }

        /*public void DestroyEntity(Entity entity)
        {
            openEntityHandles.Enqueue(entity.handle);

        }*/

        /// <summary>
        /// Link a component to an entity
        /// </summary>
        /// <typeparam name="T">The type of component to add</typeparam>
        /// <param name="entity">The entity handle</param>
        /// <param name="component">A component instance of type T</param>
        public void Add<T>(Entity entity, T component)
            where T : struct, IComponent
        {
            var affectedGroups = componentGroups.Where(g => g.Contains(component.GetType()));
            foreach (var group in affectedGroups)
            {
                group.AddIfDoesNotExist(entity);
            }
        }

        /// <summary>
        /// Unlink a component from an entity
        /// </summary>
        /// <typeparam name="T">The type of the component to unlink</typeparam>
        /// <param name="entity">The target entity handle</param>
        public void Remove<T>(Entity entity)
            where T : struct, IComponent
        {
            var affectedGroups = componentGroups.Where(g => g.Contains(typeof(T)));
            foreach (var group in affectedGroups)
            {
                group.RemoveIfExists(entity);
            }
        }

        /// <summary>
        /// Get a reference to a component
        /// </summary>
        /// <typeparam name="T">The type of component</typeparam>
        /// <param name="entity">The entity linked to the component</param>
        /// <returns>component reference</returns>
        public ref T Get<T>(Entity entity)
            where T : struct, IComponent
            => ref (componentContainerMap[typeof(T)] as ComponentContainer<T>)[entity];

        /// <summary>
        /// Send a message.
        /// All systems responding to the type of the message will execute.
        /// </summary>
        /// <param name="message">The message to send</param>
        public void Post(IMessage message)
        {
            var systems = systemMessageMap[message.GetType()];
            var injectors = systems.SelectMany(system => groupInjectorMap[system]);
            Parallel.ForEach(injectors, injector => injector.Inject(componentContainerMap));
            Parallel.ForEach(systems, system => system.Execute(this, message));
            Parallel.ForEach(injectors, injector => injector.Unpack(componentContainerMap));
        }

        /// <summary>
        /// Create a world instance.
        /// Multiple worlds can be created if desired.
        /// Will look for and register all loaded systems.
        /// System instances are unique for each world.
        /// </summary>
        /// <returns>New world instance</returns>
        public static World Create()
        {
            var world = new World();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(asm => asm.DefinedTypes);

            var resourceMap = new Dictionary<Type, object>();

            var systemTypes = types
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(System<>));

            var componentTypes = types
                .Where(type => type.IsValueType && type.GetInterfaces().Contains(typeof(IComponent)));

            var systems = new List<ISystem>();

            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            CreateComponentContainers();
            CreateSystems();

            world.systemMessageMap = systems
                .ToLookup(system => system.GetType().BaseType.GetGenericArguments()[0]) as Lookup<Type, ISystem>;
            return world;

            void CreateComponentContainers()
            {
                foreach (var componentType in componentTypes)
                {
                    var componentContainerType = typeof(ComponentContainer<>).MakeGenericType(new Type[] { componentType });
                    var constructor = componentContainerType.GetConstructor(new Type[] { typeof(int) });
                    world.componentContainerMap.Add(componentType, constructor.Invoke(new object[] { initialSize }) as IComponentContainer);
                }
            }

            void CreateSystems()
            {
                foreach (var systemType in systemTypes)
                {
                    var systemInstance = systemType.GetConstructor(Type.EmptyTypes).Invoke(null);
                    systems.Add(systemInstance as ISystem);

                    // Find all fields for resource injection
                    var resourceFields =
                        systemType.GetFields(bindingFlags)
                        .Where(fi => fi.GetCustomAttribute(typeof(ResourceAttribute)) != null);
                    InjectResources(systemInstance, resourceFields);

                    // Find groups for component injection
                    var groupFields =
                        systemType.GetFields(bindingFlags)
                        .Where(fi => fi.GetCustomAttribute(typeof(GroupAttribute)) != null);

                    var groupInjectors = new List<GroupInjector>();
                    CreateComponentGroups();
                    world.groupInjectorMap.Add(systemInstance as ISystem, groupInjectors);

                    void CreateComponentGroups()
                    {
                        foreach (var groupField in groupFields)
                        {
                            var groupFieldType = groupField.FieldType;
                            var groupFieldInstance = groupFieldType.GetConstructor(Type.EmptyTypes).Invoke(null);
                            groupField.SetValue(systemInstance, groupFieldInstance);

                            var fields = groupFieldType.GetFields(bindingFlags);
                            var lengthField = groupFieldType.GetField("length");
                            var entityField = fields.Where(fi => fi.FieldType == typeof(Entity[])).SingleOrDefault();
                            var genericFields = fields
                                .Where(fi => fi.FieldType.IsGenericType);
                            var readFields = genericFields
                                .Where(fi => fi.FieldType.GetGenericTypeDefinition() == typeof(RoArray<>));
                            var writeFields = genericFields
                                .Where(fi => fi.FieldType.GetGenericTypeDefinition() == typeof(RwArray<>));

                            var readTypes = readFields
                                .Select(fi => fi.FieldType.GetGenericArguments()[0]).ToArray();
                            var writeTypes = writeFields
                                .Select(fi => fi.FieldType.GetGenericArguments()[0]).ToArray();

                            var readTuples = readTypes.Zip(readFields, (t, f) => (t, f)).ToArray();
                            var writeTuples = writeTypes.Zip(writeFields, (t, f) => (t, f)).ToArray();

                            var componentGroup = world.GetComponentGroup(readTypes, writeTypes);
                            var groupInjector = new GroupInjector(groupFieldInstance, writeTuples, readTuples, entityField, lengthField, componentGroup);
                            groupInjectors.Add(groupInjector);
                        }
                    }
                }  
            }

            void InjectResources(object systemInstance, IEnumerable<FieldInfo> resourceFields)
            {
                foreach (var resourceField in resourceFields)
                {
                    // Create or get the resource
                    if (!resourceMap.TryGetValue(resourceField.FieldType, out var resource))
                    {
                        resource = resourceField.FieldType.GetConstructor(Type.EmptyTypes).Invoke(null);
                        resourceMap.Add(resourceField.FieldType, resource);
                    }
                    // Inject the resource
                    resourceField.SetValue(systemInstance, resource);
                }
            }
        }

        private ComponentGroup GetComponentGroup(Type[] readTypes, Type[] writeTypes)
        {
            var index = componentGroups.FindIndex(componentGroup => componentGroup.IsEquivalentTo(readTypes, writeTypes));
            if (index >= 0)
            {
                return componentGroups[index];
            }
            else
            {
                var newComponentGroup = new ComponentGroup(readTypes, writeTypes, size);
                componentGroups.Add(newComponentGroup);
                return newComponentGroup;
            }
        }


    }


}
