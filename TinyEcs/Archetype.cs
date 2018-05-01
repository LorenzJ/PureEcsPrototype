namespace TinyEcs
{
    /// <summary>
    /// An archetype is a key to define entities that share the same type of components
    /// </summary>
    public struct Archetype : IHandle<int>
    {
        internal int handle;

        public Archetype(int handle)
        {
            this.handle = handle;
        }

        public int Handle => handle;
    }
}
