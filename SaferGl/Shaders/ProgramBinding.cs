using OpenGL;
using System;

namespace SaferGl.Shaders
{
    public struct ProgramBinding : IDisposable
    {
        public void Dispose()
        {
            Gl.UseProgram(0u);
        }
    }

    public static class ProgramBindingUtil
    {
        public static ProgramBinding Use(this ShaderProgram @this)
        {
            Gl.UseProgram(@this.Handle);
            return new ProgramBinding();
        }
    }
}
