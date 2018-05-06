using System;

namespace TinyEcs
{
    /// <summary>
    /// An archetype is used to quickly instantiate entities with the same type of components.
    /// </summary>
    public struct Archetype : IHandle<int>, IEquatable<Archetype>
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

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Archetype other && Equals(other);

        /// <inheritdoc/>
        public bool Equals(Archetype other)
        {
            return handle == other.handle;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 780127187;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + handle.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(Archetype archetype1, Archetype archetype2) => archetype1.Equals(archetype2);

        /// <inheritdoc/>
        public static bool operator !=(Archetype archetype1, Archetype archetype2) => !(archetype1 == archetype2);
    }
}
