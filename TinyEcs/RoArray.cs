namespace TinyEcs
{
    /// <summary>
    /// A read-only array of components.
    /// </summary>
    /// <typeparam name="T">Element type of the component array</typeparam>
    public struct RoArray<T>
    {
        internal T[] items;

        public RoArray(T[] items)
        {
            this.items = items;
        }

        public static implicit operator RoArray<T>(T[] items) => new RoArray<T>(items);
        public static explicit operator T[](RoArray<T> roArray) => roArray.items;

        /// <summary>
        /// Get a read-only reference to an item.
        /// </summary>
        /// <param name="i">Index of the item</param>
        /// <returns></returns>
        public ref readonly T this[int i] => ref items[i];
    }
}
