using GameGl.Core.Uniforms;
using OpenGL;
using System;
using System.Text;

namespace GameGl.Core.Shaders
{
    public struct ShaderProgram : IDisposable
    {
        private uint handle;
        public uint Handle { get => handle; }

        public ShaderProgram(uint handle)
        {
            this.handle = handle;
        }

        public static ShaderProgram LinkShaders(params IShader[] shaders)
        {
            var program = Gl.CreateProgram();
            foreach (var shader in shaders)
            {
                Gl.AttachShader(program, shader.Handle);
            }
            Gl.LinkProgram(program);
            Gl.GetProgram(program, ProgramProperty.LinkStatus, out var linkStatus);
            if (linkStatus == 0)
            {
                Gl.GetProgram(program, ProgramProperty.InfoLogLength, out var length);
                var infoLog = new StringBuilder(length);
                Gl.GetProgramInfoLog(program, length, out var _, infoLog);
                throw new ProgramLinkingException(infoLog.ToString());
            }
            return new ShaderProgram(program);
        }

        private int GetUniformLocation(string name)
        {
            var location = Gl.GetUniformLocation(handle, name);
            if (location < 0)
            {
                throw new UniformNotFoundException(name);
            }
            return location;
        }
        public FloatUniform GetFloatUniform(string name) => new FloatUniform(GetUniformLocation(name));

        public void Use() => Gl.UseProgram(handle);
        public void Dispose() => Gl.DeleteProgram(handle);
    }
}
