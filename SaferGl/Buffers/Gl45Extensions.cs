using OpenGL;
using System;

namespace SaferGl.Buffers
{
    namespace Gl45
    {
        public static class BufferExtensions
        {
            public static void BufferData<T>(this T @this, uint size, object data, BufferUsage usage)
                where T : IBuffer
                => Gl.NamedBufferData(@this.Handle, size, data, usage);

            public static void BufferData<T>(this T @this, uint size, IntPtr data, BufferUsage usage)
                where T : IBuffer
                => Gl.NamedBufferData(@this.Handle, size, data, usage);

            public static void BufferSubData<T>(this T @this, uint offset, uint size, object data)
                where T : IBuffer
                => Gl.NamedBufferSubData(@this.Handle, new IntPtr(offset), size, data);

            public static void BufferSubData<T>(this T @this, uint offset, uint size, IntPtr data)
                where T : IBuffer
                => Gl.NamedBufferSubData(@this.Handle, new IntPtr(offset), size, data);
        }
    }
}
