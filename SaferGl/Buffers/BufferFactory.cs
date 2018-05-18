using OpenGL;
using System;

namespace SaferGl.Buffers
{
    public static class BufferFactory
    {
        public static T Create<T>()
            where T : IBuffer => BufferFactory<T>.Create();

        public static T Create<T>(uint size, object data, BufferUsage usage)
            where T : IBuffer => BufferFactory<T>.Create(size, data, usage);
    }

    public static class BufferFactory<T>
        where T : IBuffer
    {
        private static readonly Func<T> factory = OglObjectFactoryFactory.Create<T>(Gl.GenBuffer);

        public static T Create() => factory();

        public static T Create(uint size, object data, BufferUsage usage)
        {
            var buffer = factory();
            using (var binding = buffer.BindBuffer())
            {
                binding.Data(size, data, usage);
            }
            return buffer;
        }
    }
}
