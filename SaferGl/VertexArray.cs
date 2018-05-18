using OpenGL;
using System;

namespace SaferGl
{
    public struct VertexArray : IHandle, IDisposable
    {
        public uint Handle { get; }

        public VertexArray(uint handle) : this()
        {
            Handle = handle;
        }

        public VertexArrayBinding BindVertexArray()
        {
            return new VertexArrayBinding(this);
        }

        public static VertexArray Create() => new VertexArray(Gl.GenVertexArray());

        public void Dispose()
        {
            Gl.DeleteVertexArrays(new uint[] { Handle });
        }
    }
}
