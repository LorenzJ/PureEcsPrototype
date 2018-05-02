using System.Numerics;
using TinyEcs;

namespace Game.Components.Transform
{
    public struct Position : IComponent
    {
        public Vector2 vector;

        public Position(Vector2 vector)
        {
            this.vector = vector;
        }
    }
}
