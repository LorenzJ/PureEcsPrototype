using System;

namespace GameGl
{
    internal class Background
    {
        uint program;
        uint vao;
        int timeUniform;

        internal static Background CreateBackground()
        {
            return new Background();
        }

        internal void Draw(float time)
        {
            
        }
    }
}