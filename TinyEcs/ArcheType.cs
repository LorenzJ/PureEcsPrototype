using System;

namespace TinyEcs
{
    /// <summary>
    /// Archetype class for faster creation of entities.
    /// </summary>
    public class Archetype : IEquatable<Archetype>
    {
        private int id;
        internal Type[] types;

        internal Archetype(int id, Type[] types)
        {
            this.id = id;
            this.types = types;
        }

        public override bool Equals(object obj) => Equals(obj as Archetype);
        public bool Equals(Archetype other) => other != null && id == other.id;
        public override int GetHashCode() => 1877310944 + id.GetHashCode();
    }
}