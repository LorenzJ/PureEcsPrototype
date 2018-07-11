using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyEcs
{
    internal class ComponentGroup
    {
        internal ArchetypeGroup[] archetypeGroups;
        private Dictionary<Type, Array2> componentsMap;
        private Array2<Entity> entities;
        private readonly Type[] componentTypes;
        private readonly Type[] tagTypes;
        internal Type[] includes;
        internal Type[] excludes;
        private readonly bool includeEntities;

        public int Count { get; private set; }

        internal ComponentGroup(ArchetypeGroup[] archetypeGroups, Type[] includes, Type[] excludes, bool includeEntities)
        {
            this.includes = includes;
            this.excludes = excludes;
            this.includeEntities = includeEntities;
            if (includeEntities)
            {
                entities = new Array2<Entity>(1);
            }
            this.archetypeGroups = archetypeGroups;
            componentTypes = includes.Where(t => t.GetInterfaces().Contains(typeof(IComponent))).ToArray();
            tagTypes = includes.Where(t => t.GetInterfaces().Contains(typeof(ITag))).ToArray();
            componentsMap = new Dictionary<Type, Array2>();
            foreach (var componentType in componentTypes)
            {
                componentsMap.Add(componentType, new Array2(componentType, 64));
            }
        }

        internal void UpdateStream()
        {
            // Early exit if there are no archetype groups
            if (archetypeGroups.Length == 0)
            {
                return;
            }
            // Get the required amount of elements
            Count = archetypeGroups.Select(a => a.Count).Aggregate((a, b) => a + b);

            // Create the arrays
            if (includeEntities)
            {
                entities.Resize(Count);
            }
            foreach (var type in componentTypes)
            {
                var arr = componentsMap[type];
                arr.Resize(Count);
                componentsMap[type] = arr;
            }

            var index = 0;
            foreach (var archetypeGroup in archetypeGroups)
            {
                foreach (var type in componentTypes)
                {
                    // Copy the archetype group's component to the streams
                    Array.Copy(archetypeGroup.GetComponents(type), 0, componentsMap[type].Data, index, archetypeGroup.Count);
                }
                if (includeEntities)
                {
                    Array.Copy(archetypeGroup.GetEntities(), 0, entities.Data, index, archetypeGroup.Count);
                }
                // Advance the streams
                index += archetypeGroup.Count;
            }
        }

        /// <summary>
        /// Gets an array of components meant for read operations only.
        /// </summary>
        /// <typeparam name="T">The type of components to read</typeparam>
        /// <returns>Read-only array of T</returns>
        public RoData<T> GetRead<T>()
            where T : struct, IComponent => Array2.AsRoStream<T>(componentsMap[typeof(T)]);

        internal Array GetRead(Type type) => componentsMap[type].Data;
        internal Array GetWrite(Type type)
        {
            Lock(type);
            return componentsMap[type].Data;
        }

        private void Lock(Type type)
        {
            foreach (var archetypeGroup in archetypeGroups)
            {
                archetypeGroup.LockForWriting(type);
            }
        }

        /// <summary>
        /// Gets an array of components meant for read/write operations.
        /// </summary>
        /// <typeparam name="T">The type of components</typeparam>
        /// <returns>Mutable array of T</returns>
        public RwData<T> GetWrite<T>()
            where T : struct, IComponent
        {
            Lock(typeof(T));
            return Array2.AsRwStream<T>(componentsMap[typeof(T)]);//new RwDataStream<T>(componentsMap[typeof(T)] as T[]);
        }

        public void WriteAndUnlock(params Type[] types)
        {
            var index = 0;
            foreach (var archetypeGroup in archetypeGroups)
            {
                foreach (var type in types)
                {
                    // Copy the stream back to the archetype group
                    Array.Copy(componentsMap[type].Data, index, archetypeGroup.GetComponents(type), 0, archetypeGroup.Count);
                    // Unlock the component for the archetype
                    archetypeGroup.Unlock(type);
                }
                // Advance the streams
                index += archetypeGroup.Count;
            }
        }
        
        public RoData<Entity> Entities => Array2.AsRoStream(entities);
    }
}
