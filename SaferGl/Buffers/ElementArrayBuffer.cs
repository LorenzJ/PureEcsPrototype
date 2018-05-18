using OpenGL;

namespace SaferGl.Buffers
{
    public struct ElementArrayBuffer : IBuffer
    {
        public uint Handle { get; }
        public BufferTarget BufferTarget => BufferTarget.ElementArrayBuffer;

        public ElementArrayBuffer(uint handle) : this()
        {
            Handle = handle;
        }

        public void Dispose() => this.Delete();
    }
}
