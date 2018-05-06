using OpenGL;
using System;

namespace GameGl.Core.Textures
{
    public interface ITexture : IBindable, IDisposable
    {
        uint Handle { get; }

        TextureMagFilter MagFilter { get; set; }
        TextureMinFilter MinFilter { get; set; }
        TextureWrapMode WrapS { get; set; }
        TextureWrapMode WrapT { get; set; }
    }

    internal static class TextureUtil
    {
        private static T GetIntTexParameter<T>(TextureTarget textureTarget, GetTextureParameter textureParameter)
        {
            Gl.GetTexParameter(textureTarget, textureParameter, out int value);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static TextureMagFilter GetMagFilter(TextureTarget textureTarget)
            => GetIntTexParameter<TextureMagFilter>(textureTarget, GetTextureParameter.TextureMagFilter);

        public static void SetMagFilter(TextureTarget textureTarget, TextureMagFilter value) 
            => Gl.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, (int)value);

        public static TextureMinFilter GetMinFilter(TextureTarget textureTarget)
            => GetIntTexParameter<TextureMinFilter>(textureTarget, GetTextureParameter.TextureMinFilter);

        public static void SetMinFilter(TextureTarget textureTarget, TextureMinFilter value) 
            => Gl.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, (int)value);

        internal static TextureWrapMode GetWrapS(TextureTarget textureTarget)
            => GetIntTexParameter<TextureWrapMode>(textureTarget, GetTextureParameter.TextureWrapS);

        internal static void SetWrapS(TextureTarget texture2d, TextureWrapMode value)
            => Gl.TexParameter(texture2d, TextureParameterName.TextureWrapS, (int)value);

        internal static TextureWrapMode GetWrapT(TextureTarget texture2d)
            => GetIntTexParameter<TextureWrapMode>(texture2d, GetTextureParameter.TextureWrapT);

        internal static void SetWrapT(TextureTarget texture2d, TextureWrapMode value)
            => Gl.TexParameter(texture2d, TextureParameterName.TextureWrapT, (int)value);

    }
}
