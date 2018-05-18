using OpenGL;
using SaferGl.Buffers;

namespace GameGl
{
    public class Quad
    {
        private ArrayBuffer vbo;
        private static Quad instance;

        static Quad()
        {
            var vertices = new float[]
            {
                1f, -1f, 1f, 1f, -1f, -1f, -1f, 1f
            };

            instance = new Quad
            {
                vbo = BufferFactory.Create<ArrayBuffer>(sizeof(float) * (uint)vertices.Length, vertices, BufferUsage.StaticDraw)
            };
        }

        public static ArrayBuffer VertexBuffer => instance.vbo;
    }
}
