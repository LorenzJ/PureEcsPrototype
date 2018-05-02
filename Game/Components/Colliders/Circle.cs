using OpenGL;
using System.Numerics;
using TinyEcs;

namespace Game.Components.Colliders
{
    public struct Circle : IComponent
    {
        public Vector2 offset;
        public float radius;

        public Circle(Vector2 offset, float radius)
        {
            this.offset = offset;
            this.radius = radius;
        }
    }
}
