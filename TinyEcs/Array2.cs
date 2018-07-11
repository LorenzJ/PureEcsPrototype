using System;
using System.Diagnostics;

namespace TinyEcs
{
    internal static class Array2Util
    {
        public static int NextPowerOf2(int v)
        {
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;
            return v;
        }

        public static void Resize(ref Array array, int size)
        {
            var type = array.GetType().GetElementType();
            var newArray = Array.CreateInstance(type, size);
            Array.Copy(array, newArray, Math.Min(array.Length, newArray.Length));
            array = newArray;
        }
    }

    internal struct Array2<T>
        where T : struct
    {
        private T[] items;

        public Array2(int desiredSize)
        {
            items = new T[Array2Util.NextPowerOf2(desiredSize)];
        }

        ref T this[int index] => ref items[index];

        public T[] Data => items;

        public void Resize(int desiredSize)
        {
            var size = Array2Util.NextPowerOf2(desiredSize);
            if (size == items.Length)
            {
                return;
            }
            Array.Resize(ref items, size);
        }
    }

    internal struct Array2
    {
        private Array items;

        public Array2(Type elementType, int desiredSize)
        {
            items = Array.CreateInstance(elementType, Array2Util.NextPowerOf2(desiredSize));
        }

        public void Resize(int desiredSize)
        {
            var size = Array2Util.NextPowerOf2(desiredSize);
            if (size == items.Length)
            {
                return;
            }
            Array2Util.Resize(ref items, size);
            Debug.Assert(items.Length == size);
        }

        public Array Data => items;

        public static RoData<T> AsRoStream<T>(Array2<T> array)
            where T : struct, IData => new RoData<T>(array.Data as T[]);

        public static RoData<T> AsRoStream<T>(Array2 array)
            where T : struct, IData => new RoData<T>(array.Data as T[]);

        public static RwData<T> AsRwStream<T>(Array2<T> array)
            where T : struct, IComponent => new RwData<T>(array.Data as T[]);

        public static RwData<T> AsRwStream<T>(Array2 array) 
            where T : struct, IComponent => new RwData<T>(array.Data as T[]);
    }
}
