using OpenGL;
using System.Numerics;
using TinyEcs;

namespace Game.Components.Transform
{
    public struct Rotation : IComponent
    {
        public Vector2 vector;

        public Rotation(Vector2 vector)
        {
            this.vector = vector;
        }
    }
}
