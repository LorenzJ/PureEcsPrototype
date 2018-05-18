using OpenGL;
using System;

namespace SaferGl.Textures
{
    public struct TextureBinding<T> : IDisposable
        where T : ITexture
    {
        public static TextureTarget TextureTarget { get; }

        static TextureBinding()
        {
            TextureTarget = default(T).TextureTarget;
        }

        public TextureBinding(T texture)
        {
            Gl.BindTexture(TextureTarget, texture.Handle);
        }

        public static TextureBinding<T> Bind(T texture)
        {
            Gl.BindTexture(TextureTarget, texture.Handle);
            return new TextureBinding<T>();
        }

        public int GetIntParameter(GetTextureParameter parameter)
        {
            Gl.GetTexParameter(TextureTarget, parameter, out int value);
            return value;
        }

        public void SetParameter(TextureParameterName parameter, int value)
            => Gl.TexParameter(TextureTarget, parameter, value);

        public void GenerateMipmap() => Gl.GenerateMipmap(TextureTarget);

        public void Dispose() => Gl.BindTexture(TextureTarget, 0u);
    }

    public static class TextureBinding2D
    {
        public static void Image(this TextureBinding<Texture2D> @this,
            int level,
            InternalFormat internalFormat,
            int width,
            int height,
            int border,
            PixelFormat format,
            PixelType type,
            IntPtr data)
        {
            Gl.TexImage2D(TextureBinding<Texture2D>.TextureTarget, level, internalFormat, width, height, border, format, type, data);
        }

        public static void Image(this TextureBinding<Texture2D> @this,
            int level, 
            InternalFormat internalFormat, 
            int width, 
            int height, 
            int border, 
            PixelFormat format, 
            PixelType type,
            object data)
        {
            Gl.TexImage2D(TextureBinding<Texture2D>.TextureTarget, level, internalFormat, width, height, border, format, type, data);
        }
    }

    public static class TextureBinding1D
    {
        public static void Image(this TextureBinding<Texture1D> @this,
            int level,
            InternalFormat internalFormat,
            int width,
            int border,
            PixelFormat format,
            PixelType type,
            IntPtr data)
        {
            Gl.TexImage1D(TextureBinding<Texture1D>.TextureTarget, level, internalFormat, width, border, format, type, data);
        }

        public static void Image(this TextureBinding<Texture1D> @this,
            int level,
            InternalFormat internalFormat,
            int width,
            int border,
            PixelFormat format,
            PixelType type,
            object data)
        {
            Gl.TexImage1D(TextureBinding<Texture1D>.TextureTarget, level, internalFormat, width, border, format, type, data);
        }
    }

    public static class TextureBindingUtil
    {


        public static TextureWrapMode GetWrapR<T>(this TextureBinding<T> @this)
            where T : ITexture3D => (TextureWrapMode)@this.GetIntParameter(GetTextureParameter.TextureWrapR);

        public static TextureWrapMode GetWrapS<T>(this TextureBinding<T> @this)
            where T : ITexture1D => (TextureWrapMode)@this.GetIntParameter(GetTextureParameter.TextureWrapS);

        public static TextureWrapMode GetWrapT<T>(this TextureBinding<T> @this)
            where T : ITexture2D => (TextureWrapMode)@this.GetIntParameter(GetTextureParameter.TextureWrapT);

        public static void SetWrapR<T>(this TextureBinding<T> @this, TextureWrapMode wrapMode)
            where T : ITexture3D => @this.SetParameter(TextureParameterName.TextureWrapR, (int)wrapMode);

        public static void SetWrapS<T>(this TextureBinding<T> @this, TextureWrapMode wrapMode)
            where T : ITexture1D => @this.SetParameter(TextureParameterName.TextureWrapS, (int)wrapMode);

        public static void SetWrapT<T>(this TextureBinding<T> @this, TextureWrapMode wrapMode)
            where T : ITexture2D => @this.SetParameter(TextureParameterName.TextureWrapT, (int)wrapMode);

        public static TextureMagFilter GetMagFilter<T>(this TextureBinding<T> @this)
            where T : ITexture => (TextureMagFilter)@this.GetIntParameter(GetTextureParameter.TextureMagFilter);

        public static TextureMinFilter GetMinFilter<T>(this TextureBinding<T> @this)
            where T : ITexture => (TextureMinFilter)@this.GetIntParameter(GetTextureParameter.TextureMinFilter);
    }
}
