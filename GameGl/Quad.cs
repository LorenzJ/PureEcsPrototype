﻿using GameGl.Core.Buffers;
using OpenGL;

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
                0.5f, -0.5f, 0.5f, 0.5f, -0.5f, -0.5f, -0.5f, 0.5f
            };

            instance = new Quad
            {
                vbo = ArrayBuffer.Create(vertices, sizeof(float) * vertices.Length)
            };
        }

        public static ArrayBuffer VertexBuffer => instance.vbo;
    }
}
