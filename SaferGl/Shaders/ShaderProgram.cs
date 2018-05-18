using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaferGl.Shaders
{
    public struct ShaderProgram : IHandle, IDisposable
    {
        public uint Handle { get; }

        public ShaderProgram(uint handle) : this()
        {
            Handle = handle;
        }

        public static ShaderProgram Create() => new ShaderProgram(Gl.CreateProgram());

        public static ShaderProgram LinkShaders(IEnumerable<IShader> shaders)
        {
            var program = Create();
            foreach (var shader in shaders)
            {
                program.Attach(shader);
            }
            program.Link();
            return program;
        }

        public static ShaderProgram LinkShaders(params IShader[] shaders) => LinkShaders(shaders.AsEnumerable());

        public void Attach<T>(T shader)
            where T : IShader => Gl.AttachShader(Handle, shader.Handle);

        public void Link() => Gl.LinkProgram(Handle);

        public int GetLinkStatus()
        {
            Gl.GetProgram(Handle, ProgramProperty.LinkStatus, out var status);
            return status;
        }

        public int GetInfoLogLength()
        {
            Gl.GetProgram(Handle, ProgramProperty.InfoLogLength, out var length);
            return length;
        }

        public string GetInfoLog()
        {
            var length = GetInfoLogLength();
            var infoLog = new StringBuilder(length);
            Gl.GetProgramInfoLog(Handle, length, out var _, infoLog);
            return infoLog.ToString();
        }

        public int GetUniformLocation(string name) => Gl.GetUniformLocation(Handle, name);

        public void Dispose() => Gl.DeleteProgram(Handle);
    }
}
