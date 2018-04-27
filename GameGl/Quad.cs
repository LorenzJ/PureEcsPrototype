using OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameGl
{
    public class Quad
    {
        private uint vbo;
        private static Quad instance;

        private Quad(uint vbo)
        {
            this.vbo = vbo;
        }

        static Quad()
        {
            var vertices = new float[]
            {
                0.5f, -0.5f, 0.5f, 0.5f, -0.5f, -0.5f, -0.5f, 0.5f
            };

            var vbo = Gl.CreateBuffer();
            Gl.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            Gl.BufferData(BufferTarget.ArrayBuffer, (uint)(sizeof(float) * vertices.Length), vertices, BufferUsage.StaticDraw);
            Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
            instance = new Quad(vbo);
        }

        public static uint GetVertexBuffer()
        {
            return instance.vbo;
        }
    }
}
