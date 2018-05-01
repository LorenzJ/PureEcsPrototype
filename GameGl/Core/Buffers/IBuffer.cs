using OpenGL;
using System;

namespace GameGl.Core.Buffers
{
    public interface IBuffer : IBindable, IDisposable
    {
        uint Handle { get; }
    }

    internal static class BufferUtil
    {
        private static uint CreateAndBind(BufferTarget bufferTarget)
        {
            var handle = Gl.CreateBuffer();
            Gl.BindBuffer(bufferTarget, handle);
            return handle;
        }
        public static uint CreateEmpty(int size, BufferTarget bufferTarget, BufferUsage bufferUsage)
        {
            var handle = CreateAndBind(bufferTarget);
            Gl.BufferData(bufferTarget, (uint)size, null, bufferUsage);
            Gl.BindBuffer(bufferTarget, 0);
            return handle;
        }

        public static uint Create(object obj, int size, BufferTarget bufferTarget, BufferUsage bufferUsage)
        {
            var handle = CreateAndBind(bufferTarget);
            Gl.BufferData(bufferTarget, (uint)size, obj, bufferUsage);
            Gl.BindBuffer(bufferTarget, 0);
            return handle;
        }

        public static uint Create(IntPtr ptr, int size, BufferTarget bufferTarget, BufferUsage bufferUsage)
        {
            var handle = CreateAndBind(bufferTarget);
            Gl.BufferData(bufferTarget, (uint)size, ptr, bufferUsage);
            Gl.BindBuffer(bufferTarget, 0);
            return handle;
        }
    }
}
