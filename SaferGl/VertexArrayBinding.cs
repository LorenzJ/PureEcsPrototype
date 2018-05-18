using OpenGL;
using System;

namespace SaferGl
{
    public struct VertexArrayBinding : IDisposable
    {
        public VertexArrayBinding(VertexArray vertexArray)
        {
            Gl.BindVertexArray(vertexArray.Handle);
        }

        public void SetAttributePointer(uint index, int size, VertexAttribType attribType, bool normalized, int stride, int offset)
            => Gl.VertexAttribPointer(index, size, attribType, normalized, stride, new IntPtr(offset));

        public void EnableAttribute(uint index)
            => Gl.EnableVertexAttribArray(index);

        public void DisableAttribute(uint index)
            => Gl.DisableVertexAttribArray(index);

        public void Dispose() => Gl.BindVertexArray(0u);
    }
}
