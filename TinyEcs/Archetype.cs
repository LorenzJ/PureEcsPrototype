using System;

namespace TinyEcs
{
    public class Archetype : IEquatable<Archetype>
    {
        private int id;
        
        internal Archetype(int id)
        {
            this.id = id;
        }

        public override bool Equals(object obj) => Equals(obj as Archetype);
        public bool Equals(Archetype other) => other != null && id == other.id;
        public override int GetHashCode() => 1877310944 + id.GetHashCode();
    }
}