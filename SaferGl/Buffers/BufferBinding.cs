using OpenGL;
using System;

namespace SaferGl.Buffers
{
    public struct BufferBinding<T> : IDisposable
        where T : IBuffer
    {
        public static BufferTarget BufferTarget = default(T).BufferTarget;

        public BufferBinding(T buffer)
        {
            Gl.BindBuffer(BufferTarget, buffer.Handle);
        }

        public void Data(uint size, object data, BufferUsage usage)
            => Gl.BufferData(BufferTarget, size, data, usage);

        public void Data(uint size, IntPtr data, BufferUsage usage)
            => Gl.BufferData(BufferTarget, size, data, usage);

        public void SubData(uint offset, uint size, object data)
            => Gl.BufferSubData(BufferTarget, new IntPtr(offset), size, data);

        public void SubData(uint offset, uint size, IntPtr data)
            => Gl.BufferSubData(BufferTarget, new IntPtr(offset), size, data);

        public IntPtr Map(BufferAccess access)
            => Gl.MapBuffer(BufferTarget, access);

        public IntPtr MapRange(uint offset, uint length, BufferAccess access)
            => Gl.MapBufferRange(BufferTarget, new IntPtr(offset), length, (uint)access);

        public void Dispose() => Gl.BindBuffer(BufferTarget, 0u);
    }
}
