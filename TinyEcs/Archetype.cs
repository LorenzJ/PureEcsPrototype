namespace TinyEcs
{
    /// <summary>
    /// An archetype is a key to define entities that share the same type of components
    /// </summary>
    public struct Archetype : IHandle<int>
    {
        internal int handle;

        internal Archetype(int handle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// A handle representing this archetype.
        /// </summary>
        public int Handle => handle;
    }
}
