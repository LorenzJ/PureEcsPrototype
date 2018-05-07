using OpenGL;
using System;

namespace GameGl.Core.Textures
{
    public class Texture2d : ITexture
    {
        private uint handle;
        public uint Handle => handle;

        internal Texture2d(uint handle)
        {
            this.handle = handle;
        }

        public static Texture2d Create()
        {
            return new Texture2d(Gl.GenTexture());
        }

        public void Image(object obj, int level, InternalFormat internalFormat, int width, int height, int border, PixelFormat pixelFormat, PixelType pixelType) 
            => Gl.TexImage2D(TextureTarget.Texture2d, level, internalFormat, width, height, border, pixelFormat, pixelType, obj);

        public void Image(IntPtr ptr, int level, InternalFormat internalFormat, int width, int height, int border, PixelFormat pixelFormat, PixelType pixelType) 
            => Gl.TexImage2D(TextureTarget.Texture2d, level, internalFormat, width, height, border, pixelFormat, pixelType, ptr);

        public void GenerateMipmap() 
            => Gl.GenerateMipmap(TextureTarget.Texture2d);

        public static Texture2d FromObject(object obj, InternalFormat internalFormat, int width, int height, int border, PixelFormat pixelFormat, PixelType pixelType)
        {
            var handle = Gl.GenTexture();
            Gl.BindTexture(TextureTarget.Texture2d, handle);
            Gl.TexImage2D(TextureTarget.Texture2d, 0, internalFormat, width, height, border, pixelFormat, pixelType, obj);
            return new Texture2d(handle);
        }

        public static Texture2d FromPointer(IntPtr ptr, InternalFormat internalFormat, int width, int height, int border, PixelFormat pixelFormat, PixelType pixelType)
        {
            var handle = Gl.GenTexture();
            Gl.BindTexture(TextureTarget.Texture2d, handle);
            Gl.TexImage2D(TextureTarget.Texture2d, 0, internalFormat, width, height, border, pixelFormat, pixelType, ptr);
            return new Texture2d(handle);
        }

        public TextureWrapMode WrapS
        {
            get => TextureUtil.GetWrapS(TextureTarget.Texture2d);
            set => TextureUtil.SetWrapS(TextureTarget.Texture2d, value);
        }

        public TextureWrapMode WrapT
        {
            get => TextureUtil.GetWrapT(TextureTarget.Texture2d);
            set => TextureUtil.SetWrapT(TextureTarget.Texture2d, value);
        }

        public TextureMinFilter MinFilter
        {
            get => TextureUtil.GetMinFilter(TextureTarget.Texture2d);
            set => TextureUtil.SetMinFilter(TextureTarget.Texture2d, value);
        }

        public TextureMagFilter MagFilter
        {
            get => TextureUtil.GetMagFilter(TextureTarget.Texture2d);
            set => TextureUtil.SetMagFilter(TextureTarget.Texture2d, value);
        }

        public void Bind()
        {
            Gl.BindTexture(TextureTarget.Texture2d, handle);
        }

        public void Dispose()
        {
            Gl.DeleteTextures(new uint[] { handle });
        }

        public void Unbind()
        {
            Gl.BindTexture(TextureTarget.Texture2d, 0u);
        }
    }
}
