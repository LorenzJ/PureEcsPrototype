using OpenGL;
using System;

namespace GameGl.Core.Buffers
{
    public class ArrayBuffer : IBuffer
    {
        private uint handle;
        public uint Handle => handle;

        private ArrayBuffer(uint handle)
        {
            this.handle = handle;
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

        public void Bind() => Gl.BindBuffer(BufferTarget.ArrayBuffer, handle);
        public void Unbind() => Gl.BindBuffer(BufferTarget.ArrayBuffer, 0u);
        public void Dispose() => Gl.DeleteBuffers(new uint[] { handle });
    }
}
