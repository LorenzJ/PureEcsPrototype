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

        public void SetAttributeDivisor(uint index, uint divisor)
            => Gl.VertexAttribDivisor(index, divisor);

        public void DrawArrays(PrimitiveType mode, int first, int count)
            => Gl.DrawArrays(mode, first, count);

        public void DrawArraysInstanced(PrimitiveType mode, int first, int count, int primcount)
            => Gl.DrawArraysInstanced(mode, first, count, primcount);

        public void DrawElements(PrimitiveType mode, int count, DrawElementsType elementsType, int offset)
            => Gl.DrawElements(mode, count, elementsType, new IntPtr(offset));

        public void DrawElementsInstanced(PrimitiveType mode, int count, DrawElementsType elementsType, int offset, int primcount)
            => Gl.DrawElementsInstanced(mode, count, elementsType, new IntPtr(offset), primcount);

        public void Dispose() => Gl.BindVertexArray(0u);
    }
}
