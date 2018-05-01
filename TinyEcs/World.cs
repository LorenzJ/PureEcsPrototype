using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TinyEcs
{
    /// <summary>
    /// The World class glues <see cref="Entity" />, <see cref="IComponent"/> and <see cref="ComponentSystem{T}"/> together.
    /// Use this class to create and destroy entities, get references to components and schedule systems.
    /// It also allows very basic Dependency Injection for ComponentSystems and a simple resource locator.
    /// </summary>
    public class World
    {
        /// <summary>
        /// Reverse type to archetype lookup
        /// </summary>
        private Dictionary<Type[], Archetype> typeMap;
        /// <summary>
        /// Archetype type registry
        /// </summary>
        private FlatMap<Archetype, Type[]> archetypeMap;
        /// <summary>
        /// archetype group array indexed by archetype
        /// </summary>
        private FlatMap<Archetype, ArchetypeGroup> archetypeGroupMap;
        /// <summary>
        /// Entity to archetype lookup
        /// </summary>
        private FlatMap<Entity, Archetype> entityArchetypeMap;
        /// <summary>
        /// Map entities to an archetype group
        /// </summary>
        private FlatMap<Entity, ArchetypeGroup> entityArchetypeGroupMap;
        /// <summary>
        /// Maps entities to an internal index in their archetype group
        /// </summary>
        private FlatMap<Entity, int> entityIndexMap;
        /// <summary>
        /// To keep track off all the component groups
        /// </summary>
        private List<ComponentGroup> componentGroups;
        /// <summary>
        /// A lookup of message type to systems
        /// </summary>
        private Lookup<Type, ISystem> systemMap;
        /// <summary>
        /// A lookup for group injectors of systems
        /// </summary>
        private Dictionary<ISystem, List<GroupInjector>> groupInjectorMap;
        /// <summary>
        /// Dictionary of dependencies
        /// </summary>
        private Dictionary<Type, object> dependencyMap;

        private int nextArchetypeId;
        private int nextEntityId;
        private Queue<int> openEntityIds;

        #region Creation
        private World(IEnumerable<ISystem> systems, Dictionary<Type, object> dependencyMap)
        {
            const int reserved = 64;

            typeMap = new Dictionary<Type[], Archetype>();
            archetypeMap = new FlatMap<Archetype, Type[]>(reserved);
            archetypeGroupMap = new FlatMap<Archetype, ArchetypeGroup>(reserved);
            entityArchetypeMap = new FlatMap<Entity, Archetype>(reserved);
            entityArchetypeGroupMap = new FlatMap<Entity, ArchetypeGroup>(reserved);
            entityIndexMap = new FlatMap<Entity, int>(reserved);

            componentGroups = new List<ComponentGroup>();
            openEntityIds = new Queue<int>();

            groupInjectorMap = new Dictionary<ISystem, List<GroupInjector>>();

            systemMap = systems
                .ToLookup(system => system.GetType().BaseType.GetGenericArguments()[0]) as Lookup<Type, ISystem>;

            this.dependencyMap = dependencyMap;
        }

        /// <summary>
        /// Create a new instance of a world.
        /// Will automatically create all ComponentSystems it can find and inject their dependencies.
        /// </summary>
        /// <returns>New instance of World</returns>
        public static World Create()
        {
            // Get all loaded assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(asm => asm.DefinedTypes);
            // Find the ComponentSystem types
            var systemTypes = types
                .Where(type => type.BaseType != null 
                    && type.BaseType.IsGenericType 
                    && type.BaseType.GetGenericTypeDefinition() == typeof(ComponentSystem<>));

            // Get the constructors
            var systemConstructors = systemTypes
                .Select(type => type.GetConstructors().Single());

            // Find the dependencies
            var systemDependencies = systemConstructors
                .Select(ci => ci.GetParameters())
                .Select(pis => pis.Select(pi => pi.ParameterType).ToArray());

            // Create the systems and inject the dependencies
            var dependencyMap = new Dictionary<Type, object>();
            var systems = new List<ISystem>(systemTypes.Count());
            foreach (var entry in systemConstructors.Zip(systemDependencies, ValueTuple.Create))
            {
                var (constructor, dependencyTypes) = entry;
                var arguments = new List<object>(dependencyTypes.Length);
                foreach (var dependencyType in dependencyTypes)
                {
                    if (!dependencyMap.TryGetValue(dependencyType, out var dependency))
                    {
                        dependency = Activator.CreateInstance(dependencyType);
                        dependencyMap.Add(dependencyType, dependency);
                    }
                    arguments.Add(dependency);
                }
                // Create and add the system
                systems.Add((ISystem)constructor.Invoke(arguments.ToArray()));
            }

            // Create the world
            var world = new World(systems, dependencyMap);
            // Add the injectors for each system after the world is constructed
            foreach (var system in systems)
            {
                // find target fields
                var injectionTargetFields = system.GetType().GetFields()
                    .Where(fi => fi.CustomAttributes.Any(attr => attr.AttributeType == typeof(GroupAttribute)))
                    .ToArray();
                // Create the instances
                var injectionTargets = injectionTargetFields
                    .Select(fi => Activator.CreateInstance(fi.FieldType))
                    .ToArray();
                for (int i = 0; i < injectionTargets.Length; i++)
                {
                    // Set the initial value of the fields
                    injectionTargetFields[i].SetValue(system, injectionTargets[i]);
                }
                // Create the injectors for the fields
                var injectors = injectionTargets.Select(obj => new GroupInjector(world, obj));
                // Register the injectors for the system
                world.groupInjectorMap.Add(system, injectors.ToList());
            }

            return world;
        }
        #endregion

        /// <summary>
        /// Gets a resource that has been injected as a dependency in a ComponentSystem.
        /// </summary>
        /// <typeparam name="T">Type of resource.</typeparam>
        /// <returns>Resource of type T</returns>
        public T GetDependency<T>() where T : class => dependencyMap[typeof(T)] as T;

        /// <summary>
        /// Post a message and let all eligible systems handle it.
        /// </summary>
        /// <param name="message">Message to post</param>
        /// <remarks>Systems will run in parallel, directly using this class in the Execute method of a system is unsafe.</remarks>
        public void Post(IMessage message)
        {
            var systems = systemMap[message.GetType()];
            var injectors = systems.SelectMany(system => groupInjectorMap[system]);
            Parallel.ForEach(injectors, injector => injector.Inject());
            Parallel.ForEach(systems, system => system.Execute(this, message));
            Parallel.ForEach(injectors, injector => injector.WriteAndUnlock());
        }

        #region Archetype creation
        /// <summary>
        /// Create a new archetype.
        /// </summary>
        /// <param name="types">Component types that make up the archetype</param>
        /// <returns>New archetype</returns>
        public Archetype CreateArchetype(params Type[] types)
        {
            var archetype = new Archetype(nextArchetypeId++);
            // Register the types for the archetype
            archetypeMap[archetype] = types;
            // Add types for reverse lookup
            typeMap.Add(types, archetype);
            // Create an archetype group for the new archetype
            archetypeGroupMap[archetype] = new ArchetypeGroup(types);
            // Update component groups
            foreach (var componentGroup in componentGroups)
            {
                componentGroup.archetypeGroups = GetArchetypeGroups(componentGroup.types);
            }
            return archetype;
        }

        /// <summary>
        /// Derive a new archetype from an existing archetype.
        /// </summary>
        /// <param name="parent">Archetype to derive from</param>
        /// <param name="types">New component types to introduce</param>
        /// <returns>New archetype derived from parent</returns>
        public Archetype CreateArchetype(Archetype parent, params Type[] types)
        {
            var parentTypes = archetypeMap[parent];
            return CreateArchetype(types.Union(parentTypes).ToArray());
        }

        /// <summary>
        /// Derive a new archetype from multiple archetypes.
        /// </summary>
        /// <param name="parents">Archetypes to derive from</param>
        /// <param name="types">New component types to introduce</param>
        /// <returns>New archetype derived from parents</returns>
        public Archetype CreateArchetype(Archetype[] parents, params Type[] types)
        {
            var parentTypes = parents.SelectMany(parent => archetypeMap[parent]);
            return CreateArchetype(types.Union(parentTypes).ToArray());
        }

        /// <summary>
        /// Create a new archetype by combining archetypes.
        /// </summary>
        /// <param name="archetypes">Archetypes to combine</param>
        /// <returns>New archetype</returns>
        public Archetype CreateArchetype(params Archetype[] archetypes)
        {
            var types = archetypes.SelectMany(archetype => archetypeMap[archetype]).Distinct();
            return CreateArchetype(types.ToArray());
        }
        #endregion

        #region Entity creation/destruction/modification
        /// <summary>
        /// Create a new entity.
        /// </summary>
        /// <param name="archetype">Archetype to base the entity on</param>
        /// <returns>A new entity adhering to the specified archetype</returns>
        public Entity CreateEntity(Archetype archetype)
        {
            var entity = new Entity(GetNextEntityId());
            // Register its archetype
            entityArchetypeMap[entity] = archetype;
            // Add it to the archetype group and map the entity to the group
            var archetypeGroup = archetypeGroupMap[archetype];
            entityArchetypeGroupMap[entity] = archetypeGroup;
            archetypeGroup.Add(entity, ref entityIndexMap);
            return entity;

            int GetNextEntityId()
            {
                // Find a free entity id
                if (openEntityIds.Count > 0)
                {
                    return openEntityIds.Dequeue();
                }
                // Generate a new one if no free ones are available
                return nextEntityId++;
            }
        }

        /// <summary>
        /// Destroy an existing entity.
        /// </summary>
        /// <param name="entity">Entity to destroy</param>
        public void DestroyEntity(Entity entity)
        {
            // Get the archetype group that the entity belongs to
            var archetypeGroup = entityArchetypeGroupMap[entity];
            // Remove the entity from the archetype group and pass the entityIndexMap to be modified to reflect changes
            archetypeGroup.Remove(entity, ref entityIndexMap);
            // Ensure that the id can be reused
            openEntityIds.Enqueue(entity.handle);
        }

        /// <summary>
        /// Get a reference to a component belonging to an entity.
        /// </summary>
        /// <typeparam name="T">The type of the component</typeparam>
        /// <param name="entity">The entity the component belongs to</param>
        /// <returns>Reference to the component</returns>
        public ref T Ref<T>(Entity entity)
            where T : struct, IComponent
        {
            // Find the entity's archetype group
            var archetypeGroup = entityArchetypeGroupMap[entity];
            // Find the index inside the group
            var index = entityIndexMap[entity];
            // Finally return a reference to the component inside the group
            return ref archetypeGroup.Ref<T>(index);
        }
        #endregion

        /// <summary>
        /// Create a new component group.
        /// </summary>
        /// <param name="types">Component types included in the group.</param>
        /// <returns>A component group with components specified by types.</returns>
        internal ComponentGroup CreateComponentGroup(params Type[] types)
        {
            // Find the archetype groups that contain all the required types
            var archetypeGroups = GetArchetypeGroups(types);
            // Create a new component group based on the selected archetype groups
            var componentGroup = new ComponentGroup(archetypeGroups, types);
            // Fill the group with the component data of all the archetype groups
            componentGroup.UpdateStream();
            componentGroups.Add(componentGroup);
            return componentGroup;
        }

        private ArchetypeGroup[] GetArchetypeGroups(Type[] types)
        {
            var archetypeGroups = new List<ArchetypeGroup>();
            for (var i = 0; i < nextArchetypeId; i++)
            {
                var types_ = archetypeMap.Get(i);
                // Check if all the required types can be found in the archetype group
                if (types.All(t => types_.Contains(t)))
                {
                    // If so, add it to the list
                    archetypeGroups.Add(archetypeGroupMap.Get(i));
                }
            }
            return archetypeGroups.ToArray();
        }

        /// <summary>
        /// Get the archetype of an entity.
        /// </summary>
        /// <param name="entity">Entity from which to look up the archetype</param>
        /// <returns>Archetype of entity</returns>
        public Archetype GetArchetype(Entity entity) => entityArchetypeMap[entity];
    }
}
