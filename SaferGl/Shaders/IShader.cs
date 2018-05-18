using OpenGL;
using System;

namespace SaferGl.Shaders
{
    public interface IShader : IHandle, IDisposable
    {
        ShaderType ShaderType { get; }
    }
}
