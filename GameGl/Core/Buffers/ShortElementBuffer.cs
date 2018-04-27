using OpenGL;
using System;

namespace GameGl.Core.Buffers
{
    public struct ShortElementBuffer : IBuffer
    {
        private uint handle;

        public uint Handle => handle;

        private ShortElementBuffer(uint handle)
        {
            this.handle = handle;
        }

        public static ShortElementBuffer Create(int size, BufferUsage bufferUsage = BufferUsage.StaticDraw)
            => new ShortElementBuffer(BufferUtil.CreateEmpty(size, BufferTarget.ElementArrayBuffer, bufferUsage));

        public static ShortElementBuffer Create(object obj, int size, BufferUsage bufferUsage = BufferUsage.StaticDraw)
            => new ShortElementBuffer(BufferUtil.Create(obj, size, BufferTarget.ElementArrayBuffer, bufferUsage));

        public static ShortElementBuffer Create(IntPtr ptr, int size, BufferUsage bufferUsage = BufferUsage.StaticDraw)
            => new ShortElementBuffer(BufferUtil.Create(ptr, size, BufferTarget.ElementArrayBuffer, bufferUsage));

        public static ShortElementBuffer Create(ushort[] elements, BufferUsage usage = BufferUsage.StaticDraw)
            => new ShortElementBuffer(BufferUtil.Create(elements, elements.Length * sizeof(ushort), BufferTarget.ElementArrayBuffer, BufferUsage.StaticDraw));

        public void Bind() => Gl.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
        public void Unbind() => Gl.BindBuffer(BufferTarget.ElementArrayBuffer, 0u);
        public void Dispose() => Gl.DeleteBuffers(new uint[] { handle });
    }
}
