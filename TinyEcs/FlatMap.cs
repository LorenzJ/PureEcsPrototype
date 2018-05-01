using System;

namespace TinyEcs
{
    internal struct FlatMap<TKey, TValue>
        where TKey : IHandle<int>
    {
        private TValue[] values;

        public FlatMap(int reserve)
        {
            values = new TValue[reserve];
        }

        public TValue this[TKey key]
        {
            get => values[key.Handle];
            set
            {
                var index = key.Handle;
                // Auto resize to fit index
                if (index >= values.Length)
                {
                    Array.Resize(ref values, NextPowerOf2(index));
                }
                values[index] = value;
            }
        }

        public TValue Get(int handle) => values[handle];

        private int NextPowerOf2(int v)
        {
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;
            return v;     
        }
    }
}
