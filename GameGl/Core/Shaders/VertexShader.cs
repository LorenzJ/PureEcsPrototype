using OpenGL;

namespace GameGl.Core.Shaders
{
    public struct VertexShader : IShader
    {
        private uint handle;
        public uint Handle => handle;

        private VertexShader(uint handle)
        {
            this.handle = handle;
        }

        public static VertexShader FromSource(params string[] source)
            => new VertexShader(ShaderUtil.CompileShader(ShaderType.VertexShader, source));

        public void Dispose() => Gl.DeleteShader(handle);
    }
}
