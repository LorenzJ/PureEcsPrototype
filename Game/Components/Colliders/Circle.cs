using OpenGL;
using TinyEcs;

namespace Game.Components.Colliders
{
    struct Circle : IComponent
    {
        public Vertex3f offset;
        public float radius;
    }
}
