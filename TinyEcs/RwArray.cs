namespace TinyEcs
{
    /// <summary>
    /// A read-write array of components.
    /// </summary>
    /// <typeparam name="T">Element type of components</typeparam>
    public struct RwArray<T>
    {
        private T[] items;

        public RwArray(T[] items)
        {
            this.items = items;
        }

        public static implicit operator RwArray<T>(T[] items) => new RwArray<T>(items);
        public static explicit operator T[](RwArray<T> rwArray) => rwArray.items;

        /// <summary>
        /// Get a reference to an item.
        /// </summary>
        /// <param name="i">Index of the item</param>
        /// <returns></returns>
        public ref T this[int i] => ref items[i];
    }
}
