using OpenGL;

namespace SaferGl.Shaders
{
    public struct VertexShader : IShader
    {
        public ShaderType ShaderType => ShaderType.VertexShader;
        public uint Handle { get; }

        public VertexShader(uint handle) : this()
        {
            Handle = handle;
        }

        public void Dispose() => this.Delete();
    }
}
