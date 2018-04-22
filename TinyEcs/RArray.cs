
namespace TinyEcs
{
    public struct RArray<T>
    {
        internal T[] items;

        public RArray(T[] items)
        {
            this.items = items;
        }

        public static implicit operator RArray<T>(T[] items) => new RArray<T>(items);

        public ref readonly T this[int i] => ref items[i];
    }
}
