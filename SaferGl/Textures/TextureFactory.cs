using OpenGL;
using SaferGl.Buffers;
using System;

namespace SaferGl.Textures
{
    public static class TextureFactory
    {
        public static T Create<T>()
            where T : ITexture => TextureFactory<T>.Create();
    }

    public static class TextureFactory<T>
        where T : ITexture
    {
        public static readonly Func<T> factory = OglObjectFactoryFactory.Create<T>(Gl.GenTexture);

        public static T Create() => factory();


    }
}
