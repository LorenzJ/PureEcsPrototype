namespace TinyEcs
{
    /// <summary>
    /// An entity is a key that defines a relationship between different components.
    /// </summary>
    public struct Entity : IHandle<int>, IData
    {
        internal int handle;

        public Entity(int handle) : this()
        {
            this.handle = handle;
        }

        public int Handle => handle;
    }
}
