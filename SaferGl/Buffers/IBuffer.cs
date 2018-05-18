using OpenGL;
using System;

namespace SaferGl.Buffers
{
    public interface IBuffer : IHandle, IDisposable
    {
        BufferTarget BufferTarget { get; }
    }
}
