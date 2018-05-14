﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyEcs
{
    internal class ArchetypeGroup
    {
        private Dictionary<Type, Array> componentsMap;

        const int initialSize = 64;
        private Entity[] entities;
        private Type[] componentTypes;
        // Should find a better solution for this.
        private readonly bool[] lockedForWriting;
        private readonly Type[] tagTypes;

        public int Count { get; private set; }

        internal ArchetypeGroup(Type[] types)
        {
            entities = new Entity[initialSize];
            tagTypes = types.Where(t => t.GetInterfaces().Contains(typeof(ITag))).ToArray();
            componentTypes = types.Where(t => t.GetInterfaces().Contains(typeof(IComponent))).ToArray();
            lockedForWriting = new bool[componentTypes.Length];
            componentsMap = new Dictionary<Type, Array>();
            foreach (var componentType in componentTypes)
            {
                componentsMap.Add(componentType, Array.CreateInstance(componentType, initialSize));
            }
        }

        internal ref T Ref<T>(int index)
            where T : struct, IComponent 
            => ref (componentsMap[typeof(T)] as T[])[index];

        internal int Add(Entity entity, ref FlatMap<Entity, int> entityIndexMap)
        {
            // Allocate more space if needed
            if (Count == entities.Length - 1)
            {
                ResizeAllArrays(entities.Length * 2);
            }
            // Always add after the last element (use count as index)
            entities[Count] = entity;
            // Register the index in the world's entity index map
            entityIndexMap[entity] = Count;

            return Count++;
        }

        private void ResizeAllArrays(int length)
        {
            Array.Resize(ref entities, length);
            foreach (var type in componentTypes)
            {
                var arr = componentsMap[type];
                Resize(ref arr, length);
                componentsMap[type] = arr;
            }
        }

        internal void Remove(Entity entity, ref FlatMap<Entity, int> entityIndexMap)
        {
            Count--;
            // Get the index of the entity that needs to be removed
            var indexToReplace = entityIndexMap[entity];
            // And replace the entity with the entity at the back of the array
            var lastEntity = entities[Count];
            entities[indexToReplace] = lastEntity;
            // Reflect the change in the component arrays
            foreach (var arrayObject in componentsMap.Values)
            {
                var arr = arrayObject as Array;
                arr.SetValue(arr.GetValue(Count), indexToReplace); // Todo: Remove boxing somehow
            }
            // Update the entityIndexMap to reflect the change
            entityIndexMap[lastEntity] = indexToReplace;
        }

        internal Entity[] GetEntities() => entities;

        internal Array GetComponents(Type type) => componentsMap[type];
        internal void LockForWriting(Type type)
        {
            var index = Array.IndexOf(componentTypes, type);
            if (lockedForWriting[index])
            {
                throw new DoubleWriteLockException($"Components of type {type} already locked for writing");
            }
            lockedForWriting[index] = true;
        }

        internal void Unlock(Type type)
        {
            var index = Array.IndexOf(componentTypes, type);
            lockedForWriting[index] = false;
        }

        private void Resize(ref Array array, int size)
        {
            var type = array.GetType().GetElementType();
            var newArray = Array.CreateInstance(type, size);
            Array.Copy(array, newArray, Math.Min(array.Length, newArray.Length));
            array = newArray;
        }

        internal void Clear()
        {
            Count = 0;
        }

        internal void Move(Entity entity, ArchetypeGroup newGroup, ref FlatMap<Entity, int> entityIndexMap)
        {
            int currentIndex = entityIndexMap[entity];
            int newIndex = newGroup.Add(entity, ref entityIndexMap);
            foreach (var type in newGroup.componentTypes)
            {
                if (componentsMap.ContainsKey(type))
                {
                    newGroup.componentsMap[type].SetValue(componentsMap[type].GetValue(currentIndex), newIndex);
                }
            }
            Remove(entity, ref entityIndexMap);
        }
    }
}