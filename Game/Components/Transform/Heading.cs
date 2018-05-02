using System.Numerics;
using TinyEcs;

namespace Game.Components.Transform
{
    public struct Heading : IComponent
    {
        public Vector2 vector;

        public Heading(Vector2 vector)
        {
            this.vector = vector;
        }
    }
}
