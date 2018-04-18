using OpenGL;
using TinyEcs;

namespace Game.Components
{
    public struct Direction : IComponent
    {
        public Vertex2f vector;

        public Direction(Vertex2f vector)
        {
            this.vector = vector;
        }
    }
}
