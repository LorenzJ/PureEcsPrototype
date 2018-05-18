using OpenGL;

namespace SaferGl.Shaders
{
    public struct FragmentShader : IShader
    {
        public ShaderType ShaderType => ShaderType.FragmentShader;
        public uint Handle { get; }

        public FragmentShader(uint handle) : this()
        {
            Handle = handle;
        }

        public void Dispose() => this.Delete();
    }
}
