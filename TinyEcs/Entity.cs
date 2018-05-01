namespace TinyEcs
{
    /// <summary>
    /// An entity is a key that defines a relationship between different components.
    /// </summary>
    public struct Entity : IHandle<int>, IData
    {
        internal int handle;

        internal Entity(int handle) : this()
        {
            this.handle = handle;
        }

        /// <summary>
        /// A handle that represents this entity.
        /// </summary>
        public int Handle => handle;
    }
}
