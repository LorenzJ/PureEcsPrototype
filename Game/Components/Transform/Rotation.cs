using System.Numerics;
using TinyEcs;

namespace Game.Components.Transform
{
    public struct Rotation : IComponent
    {
        public Vector2 Vector;

        public Rotation(Vector2 vector)
        {
            Vector = vector;
        }
    }
}
