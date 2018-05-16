using OpenGL;
using System;

namespace GameGl.Core.Buffers
{
    public class ArrayBuffer : IBuffer
    {
        public uint Handle { get; }

        private ArrayBuffer(uint handle)
        {
            Handle = handle;
        }

        public static ArrayBuffer Create(int size, BufferUsage bufferUsage = BufferUsage.StaticDraw)
            => new ArrayBuffer(BufferUtil.CreateEmpty(size, BufferTarget.ArrayBuffer, bufferUsage));

        public static ArrayBuffer Create(object obj, int size, BufferUsage bufferUsage = BufferUsage.StaticDraw)
            => new ArrayBuffer(BufferUtil.Create(obj, size, BufferTarget.ArrayBuffer, bufferUsage));

        public static ArrayBuffer Create(IntPtr ptr, int size, BufferUsage bufferUsage = BufferUsage.StaticDraw)
            => new ArrayBuffer(BufferUtil.Create(ptr, size, BufferTarget.ArrayBuffer, bufferUsage));

        public void BufferSubData(int offset, int size, object data)
            => Gl.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(offset), (uint)size, data);

        public void BufferSubData(int offset, int size, IntPtr ptr)
            => Gl.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(offset), (uint)size, ptr);

        public ArrayBufferBinding GetBinding()
        {
            Bind();
            return new ArrayBufferBinding();
        }

        public void Bind() => Gl.BindBuffer(BufferTarget.ArrayBuffer, Handle);
        public void Unbind() => Gl.BindBuffer(BufferTarget.ArrayBuffer, 0u);
        public void Dispose() => Gl.DeleteBuffers(new uint[] { Handle });
    }

    public struct ArrayBufferBinding : IDisposable
    {
        public void BufferSubData(int offset, int size, object data)
            => Gl.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(offset), (uint)size, data);

        public void BufferSubData(int offset, int size, IntPtr ptr)
            => Gl.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(offset), (uint)size, ptr);

        public void Dispose()
            => Gl.BindBuffer(BufferTarget.ArrayBuffer, 0u);
    }
}
