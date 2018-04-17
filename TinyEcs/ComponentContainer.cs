using System;
using System.Collections;
using System.Collections.Generic;

namespace TinyEcs
{
    internal class ComponentContainer<T> : IComponentContainer
        where T: struct
    {
        private T[] data;
        private BitArray live;

        private List<PooledArray<T>> pooledArrays;

        public ComponentContainer()
        {
            data = new T[1024];
            live = new BitArray(1024);
            pooledArrays = new List<PooledArray<T>>();
        }

        public void Add(Entity entity)
        {
            live.Set(entity.id, true);
        }

        public void Remove(Entity entity)
        {
            data[entity.id] = default;
            live.Set(entity.id, false);
        }

        public bool Contains(Entity entity) => live.Get(entity.id);

        internal void Resize(int newSize)
        {
            var newData = new T[newSize];
            Array.Copy(data, newData, Math.Min(data.Length, newData.Length));
            data = newData;
            var newLive = new BitArray(newSize);
            newLive.Or(live);
            live = newLive;
        }

        internal ref T Get(Entity entity) => ref data[entity.id];

        internal void Set(Entity entity, in T value)
        {
            data[entity.id] = value;
            live.Set(entity.id, true);
        }

        public T[] Pack(Entity[] entities)
        {
            var packed = GetArray(entities.Length);
            for (var i = 0; i < entities.Length; i++)
            {
                packed[i] = data[entities[i].id];
            }
            return packed.items;
        }

        public void Unpack(Entity[] entities, T[] pack)
        {
            for (var i = 0; i < entities.Length; i++)
            {
                data[entities[i].id] = pack[i];
            }
        }

        private PooledArray<T> GetArray(int length)
        {
            var array = ArrayPool<T>.Get().GetPooledArray(length);
            pooledArrays.Add(array);
            return array;
        }

        public void Flush()
        {
            foreach (var pooledArray in pooledArrays)
            {
                pooledArray.Dispose();
            }
            pooledArrays.Clear();
        }
    }
}
