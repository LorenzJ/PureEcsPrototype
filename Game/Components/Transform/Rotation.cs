using OpenGL;
using TinyEcs;

namespace Game.Components.Transform
{
    public struct Rotation : IComponent
    {
        public Vertex2f vector;

        public Rotation(Vertex2f vector)
        {
            this.vector = vector;
        }
    }
}
