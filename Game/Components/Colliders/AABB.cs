using OpenGL;
using System.Numerics;
using TinyEcs;

namespace Game.Components.Colliders
{
    public struct AABB : IComponent
    {
        public Vector2 offset;
        public Vector2 size;

        public AABB(Vector2 offset, Vector2 size)
        {
            this.offset = offset;
            this.size = size;
        }
    }
}
