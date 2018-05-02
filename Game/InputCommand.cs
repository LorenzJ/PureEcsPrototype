using System;

namespace Game
{
    [Flags]
    public enum InputCommands
    {
        Fire = 0x1,
        MoveLeft = 0x2,
        MoveUp = 0x4,
        MoveRight = 0x8,
        MoveDown = 0x10
    }
}
