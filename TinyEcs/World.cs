using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

namespace TinyEcs
{
    public class World
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
        private Lookup<Type, ISystem> systemMessageMap;

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

        public void Add<T>(Entity entity, T component)
            where T : struct, IComponent
        {
            var affectedGroups = componentGroups.Where(g => g.Contains(component.GetType()));
            foreach (var group in affectedGroups)
            {
                group.AddIfDoesNotExist(entity);
            }
        }

        public ref T Get<T>(Entity entity)
            where T : struct, IComponent => ref (componentContainerMap[typeof(T)] as ComponentContainer<T>)[entity];

        public void Post(IMessage message)
        {
            var systems = systemMessageMap[message.GetType()];
            var injectors = new List<GroupInjector>();
            foreach (var system in systems)
            {
                injectors.AddRange(groupInjectorMap[system]);
            }
            Parallel.ForEach(injectors, injector => injector.Inject(componentContainerMap));
            Parallel.ForEach(systems, system => system.Execute(this, message));
            Parallel.ForEach(injectors, injector => injector.Unpack(componentContainerMap));

        }

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

            foreach (var componentType in componentTypes)
            {
                var componentContainerType = typeof(ComponentContainer<>).MakeGenericType(new Type[] { componentType });
                var constructor = componentContainerType.GetConstructor(new Type[] { typeof(int) });
                world.componentContainerMap.Add(componentType, constructor.Invoke(new object[] { initialSize }) as IComponentContainer);
            }
            foreach (var systemType in systemTypes)
            {
                var systemInstance = systemType.GetConstructor(Type.EmptyTypes).Invoke(null);
                systems.Add(systemInstance as ISystem);

                // Find all fields for resource injection
                var resourceFields =
                    systemType.GetFields(bindingFlags)
                    .Where(fi => fi.GetCustomAttribute(typeof(ResourceAttribute)) != null);

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

                // Find groups for component injection
                var groupFields =
                    systemType.GetFields(bindingFlags)
                    .Where(fi => fi.GetCustomAttribute(typeof(GroupAttribute)) != null);

                var groupInjectors = new List<GroupInjector>();
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
                        .Where(fi => fi.FieldType.GetGenericTypeDefinition() == typeof(RArray<>));
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
                world.groupInjectorMap.Add(systemInstance as ISystem, groupInjectors);
            }

            world.systemMessageMap = systems
                .ToLookup(system => system.GetType().BaseType.GetGenericArguments()[0]) as Lookup<Type, ISystem>;
            return world;
        }

        private ComponentGroup GetComponentGroup(Type[] readTypes, Type[] writeTypes)
        {
            var index = componentGroups.FindIndex(componentGroup => componentGroup.EquivalentTo(readTypes, writeTypes));
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

        private class GroupInjector
        {
            private object group;
            private (Type, FieldInfo, ConstructorInfo)[] writeFields;
            private (Type, FieldInfo, ConstructorInfo)[] readFields;
            private FieldInfo entityField;
            private FieldInfo lengthField;
            internal ComponentGroup componentGroup;

            private object[] arguments = new object[] { null };

            public GroupInjector(object group, (Type, FieldInfo)[] writeFields, (Type, FieldInfo)[] readFields, FieldInfo entityField, FieldInfo lengthField, ComponentGroup componentGroup)
            {
                this.group = group;
                this.entityField = entityField;
                this.lengthField = lengthField;
                this.componentGroup = componentGroup;

                var RwArrayType = typeof(RwArray<>);
                this.writeFields = new(Type, FieldInfo, ConstructorInfo)[writeFields.Length];
                for (var i = 0; i < writeFields.Length; i++)
                {
                    var (type, fieldInfo) = writeFields[i];
                    var rawArrayType = type.MakeArrayType();
                    var wrappedArrayType = RwArrayType.MakeGenericType(type);
                    var constructor = wrappedArrayType.GetConstructor(new Type[] { rawArrayType });
                    this.writeFields[i] = (type, fieldInfo, constructor);
                }
                var RArrayType = typeof(RArray<>);
                this.readFields = new(Type, FieldInfo, ConstructorInfo)[readFields.Length];
                for (var i = 0; i < readFields.Length; i++)
                {
                    var (type, fieldInfo) = readFields[i];
                    var rawArrayType = type.MakeArrayType();
                    var wrappedArrayType = RArrayType.MakeGenericType(type);
                    var constructor = wrappedArrayType.GetConstructor(new Type[] { rawArrayType });
                    this.readFields[i] = (type, fieldInfo, constructor);
                }

            }

            public void Inject(Dictionary<Type, IComponentContainer> containers)
            {
                componentGroup.Inject(containers);
                foreach (var (type, field, constructor) in writeFields)
                {
                    arguments[0] = componentGroup.GetDirectWrite(type);
                    field.SetValue(group, constructor.Invoke(arguments));
                }
                foreach (var (type, field, constructor) in readFields)
                {
                    arguments[0] = componentGroup.GetDirectRead(type);
                    field.SetValue(group, constructor.Invoke(arguments));
                }
                if (entityField != null)
                {
                    entityField.SetValue(group, componentGroup.Entities);
                }
                if (lengthField != null)
                {
                    lengthField.SetValue(group, componentGroup.Length);
                }
            }

            public void Unpack(Dictionary<Type, IComponentContainer> containers)
            {
                componentGroup.Unpack(containers);
            }

        }


    }


}
