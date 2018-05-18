using OpenGL;
using System;

namespace SaferGl.Shaders
{
    public static class ShaderFactory
    {
        public static T Create<T>()
            where T : IShader => ShaderFactory<T>.Create();

        public static T FromSource<T>(params string[] src)
            where T : IShader => ShaderFactory<T>.FromSource(src);
    }

    public static class ShaderFactory<T>
        where T : IShader
    {
        private static uint Method() => Gl.CreateShader(default(T).ShaderType);
        private readonly static Func<T> factory = OglObjectFactoryFactory.Create<T>(Method);

        public static T Create() => factory();
        public static T FromSource(params string[] src)
        {
            var shader = Create();
            Gl.ShaderSource(shader.Handle, src);
            Gl.CompileShader(shader.Handle);
            return shader;
        }
    }
}
