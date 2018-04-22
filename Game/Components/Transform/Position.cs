using OpenGL;
using TinyEcs;

namespace Game.Components.Transform
{
    public struct Position : IComponent
    {
        public Vertex2f vector;

        public Position(Vertex2f vector)
        {
            this.vector = vector;
        }
    }
}
