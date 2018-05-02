using System.Numerics;
using TinyEcs;

namespace Game.Components.Transform
{
    public struct Position : IComponent
    {
        public Vector2 Vector;

        public Position(Vector2 vector)
        {
            Vector = vector;
        }
    }
}
