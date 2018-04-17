using System;

namespace TinyEcs
{
    public struct Entity : IEquatable<Entity>
    {
        internal ushort id;
        internal ushort generation;

        internal Entity(ushort id, ushort generation)
        {
            this.id = id;
            this.generation = generation;
        }

        public override bool Equals(object obj)
            => obj is Entity other ? this == other : false;

        public bool Equals(Entity other)
            => this == other;

        public override int GetHashCode()
        {
            var hashCode = -530124137;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + generation.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Entity lhs, Entity rhs)
            => lhs.id == rhs.id && lhs.generation == rhs.generation;

        public static bool operator !=(Entity lhs, Entity rhs)
            => lhs.id != rhs.id || lhs.generation != rhs.generation;
    }
}
