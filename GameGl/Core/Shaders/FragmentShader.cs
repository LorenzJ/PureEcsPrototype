using OpenGL;

namespace GameGl.Core.Shaders
{
    public struct FragmentShader : IShader
    {
        public uint Handle { get; }

        private FragmentShader(uint handle)
        {
            Handle = handle;
        }

        public static FragmentShader FromSource(params string[] source)
            => new FragmentShader(ShaderUtil.CompileShader(ShaderType.FragmentShader, source));

        public void Dispose() => Gl.DeleteShader(Handle);
    }
}
