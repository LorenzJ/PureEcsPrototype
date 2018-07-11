using System;
using System.Diagnostics;

namespace TinyEcs
{
    /// <summary>
    /// An entity is a key that defines a relationship between different components.
    /// </summary>
    [DebuggerDisplay("Handle {Handle}")]
    public struct Entity : IHandle<int>, IData, IEquatable<Entity>
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

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is Entity other && Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(Entity other)
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
        public override string ToString()
        {
            return $"Entity {handle}";
        }

        /// <inheritdoc/>
        public static bool operator ==(Entity entity1, Entity entity2)
        {
            return entity1.Equals(entity2);
        }

        /// <inheritdoc/>
        public static bool operator !=(Entity entity1, Entity entity2)
        {
            return !(entity1 == entity2);
        }
    }
}
