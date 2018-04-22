using System;

namespace TinyEcs
{
    public class ArcheType : IEquatable<ArcheType>
    {
        private int id;
        
        internal ArcheType(int id)
        {
            this.id = id;
        }

        public override bool Equals(object obj) => Equals(obj as ArcheType);
        public bool Equals(ArcheType other) => other != null && id == other.id;
        public override int GetHashCode() => 1877310944 + id.GetHashCode();
    }
}