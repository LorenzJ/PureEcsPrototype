using OpenGL;

namespace GameGl.Core.Shaders
{
    public struct FragmentShader : IShader
    {
        private uint handle;
        public uint Handle => handle;

        private FragmentShader(uint handle)
        {
            this.handle = handle;
        }

        public static FragmentShader FromSource(params string[] source)
            => new FragmentShader(ShaderUtil.CompileShader(ShaderType.FragmentShader, source));

        public void Dispose() => Gl.DeleteShader(handle);
    }
}
