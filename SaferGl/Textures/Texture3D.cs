using OpenGL;

namespace SaferGl.Textures
{
    public struct Texture3D : ITexture3D
    {
        public TextureTarget TextureTarget => TextureTarget.Texture3d;
        public uint Handle { get; }

        public Texture3D(uint handle) : this()
        {
            Handle = handle;
        }

        public static Texture3D Create() => new Texture3D(Gl.GenTexture());
        public void Dispose() => this.Delete();
    }
}
