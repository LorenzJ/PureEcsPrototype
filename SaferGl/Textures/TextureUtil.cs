using OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace SaferGl.Textures
{
    public static class TextureUtil
    {
        public static TextureBinding<T> BindTexture<T>(this T @this)
            where T : ITexture => new TextureBinding<T>(@this);

        public static void Delete<T>(this T @this)
            where T : ITexture => Gl.DeleteTextures(new uint[] { @this.Handle });

        public static void Delete<T>(this IEnumerable<T> @this)
            where T : ITexture => Gl.DeleteTextures(@this.Select(t => t.Handle).ToArray());

    }
}
