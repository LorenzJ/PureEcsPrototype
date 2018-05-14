using OpenGL;

namespace GameGl.Core.Shaders
{
    public struct VertexShader : IShader
    {
        public uint Handle { get; }

        private VertexShader(uint handle)
        {
            Handle = handle;
        }

        public static VertexShader FromSource(params string[] source)
            => new VertexShader(ShaderUtil.CompileShader(ShaderType.VertexShader, source));

        public void Dispose() => Gl.DeleteShader(Handle);
    }
}
