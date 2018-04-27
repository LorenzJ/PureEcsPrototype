using GameGl.Core.Buffers;
using OpenGL;

namespace GameGl
{
    public class Quad
    {
        private ArrayBuffer vbo;
        private static Quad instance;

        private Quad(ArrayBuffer vbo)
        {
            this.vbo = vbo;
        }

        static Quad()
        {
            var vertices = new float[]
            {
                0.5f, -0.5f, 0.5f, 0.5f, -0.5f, -0.5f, -0.5f, 0.5f
            };

            var vbo = ArrayBuffer.Create(vertices, sizeof(float) * vertices.Length);
            instance = new Quad(vbo);
        }

        public static ArrayBuffer GetVertexBuffer()
        {
            return instance.vbo;
        }
    }
}
