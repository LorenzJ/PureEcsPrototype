using System.Collections.Generic;

namespace TinyEcs
{
    public class ArrayPool<T>
    {
        static ArrayPool<T> arrayPool;

        private List<PooledArray<T>> pooledArrays = new List<PooledArray<T>>();

        static ArrayPool()
        {
            arrayPool = new ArrayPool<T>();
        }

        public static ArrayPool<T> Get()
        {
            return arrayPool;
        }

        internal void Recycle(PooledArray<T> pooledArray)
        {
            pooledArrays.Add(pooledArray);
        }

        public PooledArray<T> GetPooledArray(int length)
        {
            var size = NextPowerOfTwo(length);
            var index = pooledArrays.FindIndex(array => array.items.Length == size);
            if (index >= 0)
            {
                var array = pooledArrays[index];
                array.length = length;
                pooledArrays.RemoveAt(index);
                return array;
            }
            else
            {
                var array = new PooledArray<T>(this, size, length);
                pooledArrays.Add(array);
                return array;
            }
        }

        private int NextPowerOfTwo(int v)
        {
            v--;
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
