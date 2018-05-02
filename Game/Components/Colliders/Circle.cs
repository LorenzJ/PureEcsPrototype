using System.Numerics;
using TinyEcs;

namespace Game.Components.Colliders
{
    public struct Circle : IComponent
    {
        public Vector2 Offset;
        public float Radius;

        public Circle(Vector2 offset, float radius)
        {
            Offset = offset;
            Radius = radius;
        }
    }
}
