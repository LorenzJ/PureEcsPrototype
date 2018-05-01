using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyEcs
{
    public class ComponentGroup
    {
        internal ArchetypeGroup[] archetypeGroups;
        private Dictionary<Type, Array2> componentsMap;
        private Array2<Entity> entities;
        private Type[] componentTypes;
        private Type[] tagTypes;
        private int count;
        internal Type[] types;

        public int Count => count;

        internal ComponentGroup(ArchetypeGroup[] archetypeGroups, Type[] types)
        {
            this.types = types;
            entities = new Array2<Entity>(64);
            this.archetypeGroups = archetypeGroups;
            componentTypes = types.Where(t => t.GetInterfaces().Contains(typeof(IComponent))).ToArray();
            tagTypes = types.Where(t => t.GetInterfaces().Contains(typeof(ITag))).ToArray();
            componentsMap = new Dictionary<Type, Array2>();
            foreach (var componentType in componentTypes)
            {
                componentsMap.Add(componentType, new Array2(componentType, 64));
            }
        }

        public void UpdateStream()
        {
            // Early exit if there are no archetype groups
            if (archetypeGroups.Length == 0)
            {
                return;
            }
            // Get the required amount of elements
            count = archetypeGroups.Select(a => a.Count).Aggregate((a, b) => a + b);

            // Create the arrays
            entities.Resize(count);
            foreach (var type in componentTypes)
            {
                var arr = componentsMap[type];
                arr.Resize(count);
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
                Array.Copy(archetypeGroup.GetEntities(), 0, entities.Data, index, archetypeGroup.Count);
                // Advance the streams
                index += archetypeGroup.Count;
            }
        }

        /// <summary>
        /// Gets an array of components meant for read operations only.
        /// </summary>
        /// <typeparam name="T">The type of components to read</typeparam>
        /// <returns>Read-only array of T</returns>
        public RoDataStream<T> GetRead<T>()
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
        public RwDataStream<T> GetWrite<T>()
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

        public RoDataStream<Entity> GetEntities() => Array2.AsRoStream(entities);
    }
}
