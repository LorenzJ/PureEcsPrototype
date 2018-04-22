using System;

namespace TinyEcs
{
    internal interface IComponentContainer
    {
        void Resize(int size);
        void Pack(Entity[] entities, object dest, int length);
        void Unpack(Entity[] entities, object src, int length);
    }

    internal class ComponentContainer<T> : IComponentContainer
        where T: struct, IComponent
    {
        private T[] items;

        public ComponentContainer(int size)
        {
            items = new T[size];
        }

        public ref T this[Entity entity] => ref items[entity.handle];

        void IComponentContainer.Pack(Entity[] entities, object dest, int length)
        {
            var packed = dest as T[];
            for (var i = 0; i < length; i++)
            {
                packed[i] = items[entities[i].handle];
            } 
        }

        void IComponentContainer.Unpack(Entity[] entities, object src, int length)
        {
            var packed = src as T[];
            for (var i = 0; i < length; i++)
            {
                items[entities[i].handle] = packed[i];
            }
        }

        void IComponentContainer.Resize(int size)
        {
            Array.Resize(ref items, size);
        }
    }
}
