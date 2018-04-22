namespace TinyEcs
{
    public struct RwArray<T>
    {
        private T[] items;

        public RwArray(T[] items)
        {
            this.items = items;
        }

        public static implicit operator RwArray<T>(T[] items) => new RwArray<T>(items);

        public ref T this[int i] => ref items[i];
    }
}
