using OpenGL;

namespace SaferGl.Textures
{
    public struct Texture1D : ITexture
    {
        public TextureTarget TextureTarget => TextureTarget.Texture1d;
        public uint Handle { get; }

        public Texture1D(uint handle) : this()
        {
            Handle = handle;
        }

        public static Texture1D Create() => new Texture1D(Gl.GenBuffer());

        public void Dispose() => this.Delete();
    }
}
