﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyEcs
{
    public class ComponentGroup
    {
        private int length;
        private Entity[] entities;
        private Dictionary<Type, object> readMap = new Dictionary<Type, object>();
        private Dictionary<Type, object> writeMap = new Dictionary<Type, object>();
        private HashSet<Type> componentTypes = new HashSet<Type>();
        private object[] arguments = new object[] { null };

        internal ComponentGroup(Type[] readTypes, Type[] writeTypes, int size)
        {
            foreach (var type in readTypes)
            {
                componentTypes.Add(type);
                var array = Array.CreateInstance(type, size);
                readMap.Add(type, array);
            }
            foreach (var type in writeTypes)
            {
                componentTypes.Add(type);
                var array = Array.CreateInstance(type, size);
                writeMap.Add(type, array);
            }
            entities = new Entity[size];
        }

        internal void Resize(int size)
        {
            Array.Resize(ref entities, size);
            foreach (var key in readMap.Keys.ToArray())
            {
                readMap[key] = Resize(readMap[key], size);
            }
            foreach (var key in writeMap.Keys.ToArray())
            {
                writeMap[key] = Resize(writeMap[key], size);
            }
        }

        private object Resize(object arrayObject, int size)
        {
            var array = arrayObject as Array;
            var type = array.GetType().GetElementType();
            var newArray = Array.CreateInstance(type, size);
            Array.Copy(array as Array, newArray, Math.Min(array.Length, newArray.Length));
            return newArray;
        }

        internal void Inject(Dictionary<Type, IComponentContainer> componentContainers)
        {
            foreach (var entry in readMap)
            {
                var container = componentContainers[entry.Key];
                container.Pack(entities, readMap[entry.Key], length);
            }
            foreach (var entry in writeMap)
            {
                var container = componentContainers[entry.Key];
                container.Pack(entities, writeMap[entry.Key], length);
            }
        }

        internal void Unpack(Dictionary<Type, IComponentContainer> componentContainers)
        {
            foreach (var entry in writeMap)
            {
                var container = componentContainers[entry.Key];
                container.Unpack(entities, writeMap[entry.Key], length);
            }
        }

        internal void Add(Entity entity)
        {
            if (entities.Length == length)
            {
                Resize(length * 2);
            }
            entities[length++] = entity;
            BubbleDown(entities, length);
        }

        internal void AddIfDoesNotExist(Entity entity)
        {
            var index = Array.BinarySearch(entities, 0, length, entity);
            if (index < 0)
            {
                Add(entity);
            }
        }

        internal void RemoveIfExists(Entity entity)
        {
            var index = Array.BinarySearch(entities, entity);
            if (index < 0)
            {
                return;
            }
            else
            {
                length--;
                if (index >= 0 && index != length)
                {
                    Array.Copy(entities, index + 1, entities, index, length - index);
                }
            }
        }

        public int Length => length;
        public RoArray<Entity> Entities => new RoArray<Entity>(entities);

        public RoArray<T> GetRead<T>()
            where T : struct, IComponent
            => new RoArray<T>(readMap[typeof(T)] as T[]);

        

        private void BubbleDown(Entity[] entities, int length)
        {
            for (var i = length - 1; i > 0; i--)
            {
                if (entities[i - 1] < entities[i])
                {
                    break;
                }
                var largerEntity = entities[i - 1];
                entities[i - 1] = entities[i];
                entities[i] = largerEntity;
            }
        }

        public RwArray<T> GetWrite<T>()
            where T : struct, IComponent
            => new RwArray<T>(writeMap[typeof(T)] as T[]);

        internal object GetDirectRead(Type type) => readMap[type];
        internal object GetDirectWrite(Type type) => writeMap[type];

        public bool Contains(Type componentType) => componentTypes.Contains(componentType);

        internal bool IsEquivalentTo(Type[] readTypes, Type[] writeTypes)
        {
            var innerReadTypes = readMap.Keys;
            var innerWriteTypes = writeMap.Keys;

            if (readTypes.Length != innerReadTypes.Count || writeTypes.Length != innerWriteTypes.Count)
            {
                return false;
            }
            else if (readTypes.Union(innerReadTypes).Count() != readTypes.Length)
            {
                return false;
            }
            else if (writeTypes.Union(innerWriteTypes).Count() != writeTypes.Length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
