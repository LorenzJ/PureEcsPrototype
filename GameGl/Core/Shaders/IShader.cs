using OpenGL;
using System;
using System.Diagnostics;
using System.Text;

namespace GameGl.Core.Shaders
{
    public interface IShader : IHandle, IDisposable { }

    internal static class ShaderUtil
    {
        public static uint CompileShader(ShaderType shaderType, params string[] source)
        {
            var shader = Gl.CreateShader(shaderType);
            Gl.ShaderSource(shader, source);
            Gl.CompileShader(shader);
            Gl.GetShader(shader, ShaderParameterName.CompileStatus, out var compileStatus);
            if (compileStatus == 0)
            {
                Gl.GetShader(shader, ShaderParameterName.InfoLogLength, out var length);
                var infoLog = new StringBuilder(length);
                Gl.GetShaderInfoLog(shader, length, out var _, infoLog);
                throw new ShaderCompilationException(infoLog.ToString());
            }
#if DEBUG
            Gl.GetShader(shader, ShaderParameterName.InfoLogLength, out var infoLogLength);
            if (infoLogLength > 0)
            {
                var infoLog = new StringBuilder(infoLogLength);
                Gl.GetShaderInfoLog(shader, infoLogLength, out var _, infoLog);
                Debug.Print(infoLog.ToString());
            }
#endif
            return shader;
        }
    }
}
