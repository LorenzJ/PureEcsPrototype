using GameGl.Core.Attributes;
using GameGl.Core.Buffers;
using OpenGL;
using System;

namespace GameGl.Core
{
    public struct VertexArray : IBindable, IHandle, IDisposable
    {
        public uint Handle { get; }

        internal VertexArray(uint handle)
        {
            Handle = handle;
        }

        public void Bind() => Gl.BindVertexArray(Handle);
        public void Unbind() => Gl.BindVertexArray(0u);

        public void Dispose() => Gl.DeleteVertexArrays(new uint[] { Handle });
    }

    public class VertexArrayBuilder
    {
        private VertexArray vertexArray;

        public VertexArrayBuilder()
        {
            vertexArray = new VertexArray(Gl.CreateVertexArray());
            vertexArray.Bind();
        }

        public void ChangeArrayBuffer(ArrayBuffer buffer) => buffer.Bind();

        public void SetAttribute(uint index, int size, VertexAttribType attribType, bool normalized, int stride, int offset, bool enable = true)
        {
            Gl.VertexAttribPointer(index, size, attribType, normalized, stride, new IntPtr(offset));
            if (enable)
            {
                Gl.EnableVertexAttribArray(index);
            }
        }
        public void SetAttribute(Vec2Attribute vec2Attribute, int stride, int offset, bool enable = true)
            => SetAttribute(vec2Attribute.Index, 2, VertexAttribType.Float, false, stride, offset, enable);
        public void SetAttribute(FloatAttribute floatAttribute, int stride, int offset, bool enable = true)
            => SetAttribute(floatAttribute.Index, 1, VertexAttribType.Float, false, stride, offset, enable);

        public VertexArray Build()
        {
            vertexArray.Unbind();
            return vertexArray;
        }

        public void SetAttributeDivisor(IAttribute attribute, uint divisor)
            => Gl.VertexAttribDivisor(attribute.Index, divisor);

        public void SetAttributeDivisor(uint index, uint divisor)
            => Gl.VertexAttribDivisor(index, divisor);
        
    }
}
