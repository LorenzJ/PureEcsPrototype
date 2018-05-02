using System.Numerics;
using TinyEcs;

namespace Game.Components.Transform
{
    public struct Heading : IComponent
    {
        public Vector2 Vector;

        public Heading(Vector2 vector)
        {
            Vector = vector;
        }
    }
}
