using System;

namespace TinyEcs
{
    public struct Entity : IEquatable<Entity>
    {
        internal uint id;

        internal Entity(uint id)
        {
            this.id = id;
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
            return hashCode;
        }

        public static bool operator ==(Entity lhs, Entity rhs)
            => lhs.id == rhs.id;

        public static bool operator !=(Entity lhs, Entity rhs)
            => lhs.id != rhs.id;
    }
}
