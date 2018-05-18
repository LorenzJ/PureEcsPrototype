using SaferGl.Buffers;

namespace GameGl
{
    public class Triangle
    {
        private ArrayBuffer vbo;
        private static Triangle instance;

        static Triangle()
        {
            float[] vertices =
            {
                .0f, 1f,
                -1f, -1f,
                1f, -1f
            };
            instance = new Triangle
            {
                vbo = BufferFactory.Create<ArrayBuffer>(sizeof(float) * (uint)vertices.Length, vertices, OpenGL.BufferUsage.StaticDraw)
            };
        }

        public static ArrayBuffer VertexBuffer => instance.vbo;
    }
}
