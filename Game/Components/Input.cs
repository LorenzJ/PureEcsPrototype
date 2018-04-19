using OpenGL;
using TinyEcs;

namespace Game.Components
{
    public struct Input : IComponent
    {
        public Vertex2f direction;
        public bool fire;
    }
}
