using OpenGL;

namespace SaferGl.Textures
{
    public struct Texture2D : ITexture2D
    {
        public uint Handle { get; }
        public TextureTarget TextureTarget => TextureTarget.Texture2d;

        public Texture2D(uint handle) : this()
        {
            Handle = handle;
        }

        public static Texture2D Create() => new Texture2D(Gl.GenTexture());
        public static void Unbind() => Gl.BindTexture(TextureTarget.Texture2d, 0u);
        public void Dispose() => this.Delete();
    }
}
