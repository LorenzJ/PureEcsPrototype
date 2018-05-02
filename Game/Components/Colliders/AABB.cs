using System.Numerics;
using TinyEcs;

namespace Game.Components.Colliders
{
    public struct AABB : IComponent
    {
        public Vector2 Offset;
        public Vector2 Size;

        public AABB(Vector2 offset, Vector2 size)
        {
            Offset = offset;
            Size = size;
        }
    }
}
