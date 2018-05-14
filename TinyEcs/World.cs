﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TinyEcs
{
    /// <summary>
    /// The World class glues <see cref="Entity">entities</see>, <see cref="IComponent">components</see> and <see cref="ComponentSystem{T}">systems</see> together.
    /// Use this class to create and destroy entities, get references to components and schedule systems.
    /// It also allows very basic Dependency Injection for ComponentSystems and a simple resource locator.
    /// </summary>
    public class World
    {
        // Todo: Move cold data to a sub class
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
        private readonly Lookup<Type, ISystem> systemMap;
        /// <summary>
        /// A lookup for group injectors of systems
        /// </summary>
        private Dictionary<ISystem, List<GroupInjector>> groupInjectorMap;
        /// <summary>
        /// Dictionary of dependencies
        /// </summary>
        private readonly Dictionary<Type, object> dependencyMap;
        /// <summary>
        /// Reverse type to archetype lookup
        /// </summary>
        private Dictionary<Type[], Archetype> typeMap;
        private FlatMap<Archetype, bool> archetypeExistanceMap;

        private Dictionary<Type, object> postMessages;

        private ConcurrentQueue<Action> postActions;

        private int nextArchetypeId;
        private int nextEntityId;
        private Queue<int> openEntityIds;

        private static Archetype defaultArchetype;

        /// <summary>
        /// Helper class for debugging.
        /// </summary>
        public class DebugEvents_
        {
            private readonly World world;

            internal DebugEvents_(World world)
            {
                this.world = world;
            }

            /// <summary>
            /// Event called when an entity is added and the DEBUG symbol is defined.
            /// </summary>
            public event EventHandler<Entity> EntityAdded;
            /// <summary>
            /// Event called when an entity is removed and the DEBUG symbol is defined.
            /// </summary>
            public event EventHandler<Entity> EntityRemoved;

            [Conditional("DEBUG")]
            internal void RaiseEntityAdded(Entity entity)
            {
                EntityAdded?.Invoke(world, entity);
            }

            [Conditional("DEBUG")]
            internal void RaiseEntityRemoved(Entity entity)
            {
                EntityRemoved?.Invoke(world, entity);
            }

        }
        /// <summary>
        /// DebugEvents instance for this world.
        /// </summary>
        public DebugEvents_ DebugEvents { get; private set; }

        static World()
        {
            defaultArchetype = new Archetype(0);
        }

        #region Creation
        private World(IEnumerable<ISystem> systems, Dictionary<Type, object> dependencyMap)
        {
            const int reserved = 64;

            typeMap = new Dictionary<Type[], Archetype>(new TypeArrayCompararer());
            archetypeExistanceMap = new FlatMap<Archetype, bool>(reserved);
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

            postActions = new ConcurrentQueue<Action>();
            postMessages = new Dictionary<Type, object>();

            nextArchetypeId = 1;
            nextEntityId = 0;
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

            // Call the OnLoad method on dependencies implementing IOnLoad
            foreach (var dependency in dependencyMap.Values)
            {
                if (dependency is IOnLoad onLoad)
                {
                    onLoad.OnLoad(world);
                }
            }

            // Create DebugEvents
            world.DebugEvents = new DebugEvents_(world);
            return world;
        }
        #endregion

        /// <summary>
        /// Gets a resource that has been injected as a dependency in a ComponentSystem.
        /// </summary>
        /// <typeparam name="T">Type of resource.</typeparam>
        /// <returns>Resource of type T</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the requested resource was not injected.</exception>
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

            foreach (var entry in postMessages)
            {
                var messages = entry.Value as IEnumerable;
                foreach (var postMessage in messages)
                {
                    Post(postMessage as IMessage);
                }
            }

            while (postActions.TryDequeue(out var action))
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        public void Send<T>(T message)
            where T : IMessage
        {
            var list = GetMessageList<T>();
            list.Add(message);
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messages"></param>
        public void Send<T>(T[] messages)
            where T : IMessage
        {
            var list = GetMessageList<T>();
            list.AddRange(messages);
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messages"></param>
        public void Send<T>(IEnumerable<T> messages)
            where T : IMessage
        {
            var list = GetMessageList<T>();
            list.AddRange(list);
        }

        private List<T> GetMessageList<T>() where T : IMessage
        {
            if (postMessages.TryGetValue(typeof(T), out var queueObject))
            {
                return queueObject as List<T>;
            }
            else
            {
                var list = new List<T>();
                postMessages.Add(typeof(T), list);
                return list;
            }
        }

        /// <summary>
        /// Enqueue an action to be executed synchronously after handling a message 
        /// </summary>
        /// <param name="action">The action to be executed</param>
        public void PostAction(Action action)
        {
            postActions.Enqueue(action);
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
                componentGroup.archetypeGroups = GetArchetypeGroups(componentGroup.includes, componentGroup.excludes).ToArray();
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

            // Raise an event when debugging is enabled
            DebugEvents.RaiseEntityAdded(entity);
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
        /// Create an empty entity with the default archetype.
        /// </summary>
        /// <returns>Empty entity</returns>
        public Entity CreateEntity() => CreateEntity(defaultArchetype);

        /// <summary>
        /// Destroy an existing entity.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> to destroy</param>
        public void DestroyEntity(Entity entity)
        {
            // Get the archetype group that the entity belongs to
            var archetypeGroup = entityArchetypeGroupMap[entity];
            // Remove the entity from the archetype group and pass the entityIndexMap to be modified to reflect changes
            archetypeGroup.Remove(entity, ref entityIndexMap);
            // Ensure that the id can be reused
            openEntityIds.Enqueue(entity.handle);

            // Raise event in debug mode
            DebugEvents.RaiseEntityRemoved(entity);
        }

        /// <summary>
        /// Destroy all entities adhering to an archetype.
        /// </summary>
        /// <param name="archetype">The <see cref="Entity"/> of the entities to remove</param>
        public void DestroyAll(Archetype archetype)
        {
            var archetypeGroup = archetypeGroupMap[archetype];
            archetypeGroup.Clear();
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

        /// <summary>
        /// Get a copy of a component belonging to an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="componentType">Type of the component</param>
        /// <returns>Copy of component</returns>
        /// <remarks>Prefer to use the generic version as it avoids boxing</remarks>
        public object Get(Entity entity, Type componentType)
        {
            var archetypeGroup = entityArchetypeGroupMap[entity];
            var index = entityIndexMap[entity];
            return archetypeGroup.GetComponents(componentType).GetValue(index);
        }

        /// <summary>
        /// Add a component to an entity. Will change its archetype.
        /// </summary>
        /// <typeparam name="T">Type of component to add</typeparam>
        /// <param name="entity">Target entity</param>
        public void Add<T>(Entity entity)
            where T : struct, IComponent
        {
            var currentArchetype = entityArchetypeMap[entity];
            var types = archetypeMap[currentArchetype].Concat(new Type[] { typeof(T) }).ToArray();
            MoveEntity(entity, currentArchetype, types);
        }

        private void MoveEntity(Entity entity, Archetype currentArchetype, Type[] types)
        {
            var newArchetype = typeMap[types];
            if (newArchetype == defaultArchetype)
            {
                newArchetype = CreateArchetype(types);
            }
            var oldGroup = archetypeGroupMap[currentArchetype];
            var newGroup = archetypeGroupMap[newArchetype];
            oldGroup.Move(entity, newGroup, ref entityIndexMap);
        }

        /// <summary>
        /// Remove a component from an entity, will change its archetype
        /// </summary>
        /// <typeparam name="T">Type of component to remove</typeparam>
        /// <param name="entity">Target entity</param>
        public void Remove<T>(Entity entity)
            where T : struct, IComponent
        {
            var currentArchetype = entityArchetypeMap[entity];
            var types = archetypeMap[currentArchetype].Except(new Type[] { typeof(T) }).ToArray();
            MoveEntity(entity, currentArchetype, types);
        }
        #endregion

        /// <summary>
        /// Create a new component group.
        /// </summary>
        /// <param name="includes">Component types included in the group.</param>
        /// <returns>A component group with components specified by types.</returns>
        internal ComponentGroup CreateComponentGroup(params Type[] includes)
        {
            return CreateComponentGroup(includes, new Type[] { });
        }

        internal ComponentGroup CreateComponentGroup(Type[] includes, Type[] excludes)
        {
            var archetypeGroups = GetArchetypeGroups(includes, excludes).ToArray();
            var componentGroup = new ComponentGroup(archetypeGroups, includes, excludes);
            componentGroup.UpdateStream();
            componentGroups.Add(componentGroup);
            return componentGroup;
        }


        private IEnumerable<ArchetypeGroup> GetArchetypeGroups(Type[] includes, Type[] excludes)
        {
            var includedGroups = new List<ArchetypeGroup>();
            for (var i = 1; i < nextArchetypeId; i++)
            {
                // Check if all the required types can be found in the archetype group
                if (includes.All(t => archetypeMap.Get(i).Contains(t)))
                {
                    // If so, add it to the list
                    includedGroups.Add(archetypeGroupMap.Get(i));
                }
            }
            var excludedGroups = new List<ArchetypeGroup>();
            for (var i = 1; i < nextArchetypeId; i++)
            {
                if (excludes.Any(t => archetypeMap.Get(i).Contains(t)))
                {
                    excludedGroups.Add(archetypeGroupMap.Get(i));
                }
            }
            return includedGroups.Except(excludedGroups);
        }



        /// <summary>
        /// Get the archetype of an entity.
        /// </summary>
        /// <param name="entity">Entity from which to look up the archetype</param>
        /// <returns>Archetype of entity</returns>
        public Archetype GetArchetype(Entity entity) => entityArchetypeMap[entity];

        /// <summary>
        /// Get the component types of an archetype.
        /// </summary>
        /// <param name="archetype">Archetype</param>
        /// <returns>Component types of archetype</returns>
        public Type[] GetArchetypeTypes(Archetype archetype) => archetypeMap[archetype];

        private class TypeArrayCompararer : IEqualityComparer<Type[]>
        {
            public bool Equals(Type[] x, Type[] y)
                => x.Length == y.Length && x.Union(y).Count() == y.Length;

            public int GetHashCode(Type[] types)
            {
                unchecked
                {
                    int hash = 17;
                    foreach (var type in types)
                    {
                        hash *= type.GetHashCode();
                    }
                    return hash;
                }
            }
        }
    }
}
