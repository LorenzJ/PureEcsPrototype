using OpenGL;
using System.Numerics;
using TinyEcs;

namespace Game.Components.Player
{
    public struct Input : IComponent
    {
        public Vector2 direction;
        public InputCommands commands;
    }
}
