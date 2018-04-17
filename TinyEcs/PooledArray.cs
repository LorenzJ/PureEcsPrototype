using System;

namespace TinyEcs
{
    public struct PooledArray<T> : IDisposable
    {
        private ArrayPool<T> parentPool;
        internal T[] items;
        internal int length;

        public int Length { get => length; }
        public ref T this[int index]
        {
            get => ref items[index];
        }

        internal PooledArray(ArrayPool<T> parentPool, int size, int length)
        {
            this.parentPool = parentPool;
            this.items = new T[size];
            this.length = length;
        }

        public void Dispose()
        {
            parentPool.Recycle(this);
        }

    }
}
