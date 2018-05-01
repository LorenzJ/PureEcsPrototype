using GameGl.Core.Buffers;

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
                .0f, .5f,
                -.5f, -.5f,
                .5f, -.5f
            };
            instance = new Triangle
            {
                vbo = ArrayBuffer.Create(vertices, sizeof(float) * vertices.Length)
            };
        }

        public static ArrayBuffer VertexBuffer => instance.vbo;
    }
}
