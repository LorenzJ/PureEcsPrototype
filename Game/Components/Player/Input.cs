using System.Numerics;
using TinyEcs;

namespace Game.Components.Player
{
    public struct Input : IComponent
    {
        public Vector2 Direction;
        public InputCommands Commands;
    }
}
