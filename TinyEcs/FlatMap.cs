using System;
using System.Collections.Generic;

namespace TinyEcs
{
    /// <summary>
    /// A very dumb dictionary-like type that only works with keys implementing <see cref="IHandle{T}"/> 
    /// Works like an auto-resizing array. Adds some type-safety to using arrays for lookups.
    /// </summary>
    /// <typeparam name="TKey">A key implementing <see cref="IHandle{T}"/></typeparam>
    /// <typeparam name="TValue">Any type</typeparam>
    /// <remarks>Does not support enumeration.</remarks>
    public struct FlatMap<TKey, TValue>
        where TKey : IHandle<int>
    {
        private TValue[] values;

        /// <summary>
        /// Create a new FlatMap
        /// </summary>
        /// <param name="reserve">Initial slots</param>
        public FlatMap(int reserve)
        {
            values = new TValue[reserve];
        }

        /// <summary>
        /// Get or set a value by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get a value by index
        /// </summary>
        /// <param name="handle">Index as int</param>
        /// <returns>Value</returns>
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
