using OpenGL;
using TinyEcs;

namespace Game.Components.Colliders
{
    public struct AABB : IComponent
    {
        public Vertex2f offset;
        public Vertex2f size;

        public AABB(Vertex2f offset, Vertex2f size)
        {
            this.offset = offset;
            this.size = size;
        }
    }
}
