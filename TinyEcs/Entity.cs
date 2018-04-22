using System;

namespace TinyEcs
{
    public struct Entity : IEquatable<Entity>, IComparable<Entity>
    {
        internal int handle;

        internal Entity(int handle)
        {
            this.handle = handle;
        }

        public int CompareTo(Entity other) => handle - other.handle;

        public override bool Equals(object obj) => obj is Entity other && Equals(other);
        public bool Equals(Entity other) => handle == other.handle;
        public override int GetHashCode() => unchecked(780127187 * -1521134295 + handle.GetHashCode());
        public static bool operator ==(Entity entity1, Entity entity2) => entity1.handle == entity2.handle;
        public static bool operator !=(Entity entity1, Entity entity2) => !(entity1 == entity2);
        public static bool operator <(Entity entity1, Entity entity2) => entity1.handle < entity2.handle;
        public static bool operator >(Entity entity1, Entity entity2) => entity1.handle > entity2.handle;
        public static bool operator <=(Entity entity1, Entity entity2) => entity1.handle <= entity2.handle;
        public static bool operator >=(Entity entity1, Entity entity2) => entity1.handle >= entity2.handle;
    }
}
