using OpenGL;

namespace SaferGl.Buffers
{
    public struct ArrayBuffer : IBuffer
    {
        public uint Handle { get; }
        public BufferTarget BufferTarget => BufferTarget.ArrayBuffer;

        public ArrayBuffer(uint handle)
        {
            Handle = handle;
        }
        
        public void Dispose() => this.Delete();
    }
}
