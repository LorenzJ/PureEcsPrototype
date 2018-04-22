using OpenGL;
using TinyEcs;

namespace Game.Components.Transform
{
    public struct Heading : IComponent
    {
        public Vertex2f vector;

        public Heading(Vertex2f vector)
        {
            this.vector = vector;
        }
    }
}
