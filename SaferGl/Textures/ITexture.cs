using OpenGL;
using System;

namespace SaferGl.Textures
{
    public interface ITexture : IHandle, IDisposable
    {
        TextureTarget TextureTarget { get; }
    }

    public interface ITexture1D : ITexture
    {
    }

    public interface ITexture2D : ITexture1D
    {
    }

    public interface ITexture3D : ITexture2D
    {
    }
}
