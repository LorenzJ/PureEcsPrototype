namespace TinyEcs
{
    /// <summary>
    /// A readable and writable array of components.
    /// </summary>
    /// <typeparam name="T">The type of component</typeparam>
    public struct RwDataStream<T>
        where T : struct, IComponent
    {
        private T[] items;

        /// <summary>
        /// Wrap an array as a RwDataStream.
        /// </summary>
        /// <param name="array">Array to wrap</param>
        public RwDataStream(T[] array)
        {
            items = array;
        }

        /// <summary>
        /// Access element by index.
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Reference to an element of type T</returns>
        public ref T this[int index] => ref items[index];
        /// <summary>
        /// Cast to an array. O(1) operation.
        /// Length of the array may not match the length of the stream.
        /// </summary>
        /// <param name="rw">RwDataStream to cast.</param>
        /// <returns>Array of <typeparamref name="T"/></returns>
        public static explicit operator T[](RwDataStream<T> rw) => rw.items;
    }

    /// <summary>
    /// A read-only array of components or entities.
    /// </summary>
    /// <typeparam name="T">Either the type of component or entity</typeparam>
    public struct RoDataStream<T>
        where T : struct, IData
    {
        internal T[] items;

        /// <summary>
        /// Wrap an array as a RoDataStream.
        /// </summary>
        /// <param name="array">Array to wrap</param>
        public RoDataStream(T[] array)
        {
            items = array;
        }

        /// <summary>
        /// Access element by index.
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Readonly reference to an element of type T</returns>
        public ref readonly T this[int index] => ref items[index];
        /// <summary>
        /// Cast to an array. O(1) operation.
        /// Length of the array may not match the length of the stream.
        /// </summary>
        /// <param name="ro">RoDataStream to cast.</param>
        /// <returns>Array of <typeparamref name="T"/></returns>
        public static explicit operator T[] (RoDataStream<T> ro) => ro.items;
    }
}