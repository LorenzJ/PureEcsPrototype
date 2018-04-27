using Game.Components.Transform;
using GameGl.Core;
using GameGl.Core.Attributes;
using GameGl.Core.Buffers;
using GameGl.Core.Shaders;
using GameGl.Core.Uniforms;
using GameGl.Properties;
using OpenGL;
using System;
using System.Runtime.InteropServices;
using System.Text;
using TinyEcs;

namespace GameGl
{
    public class BulletBatch
    {
        const int positionIndex = 0;
        const int offsetIndex = 1;

        VertexArray vao;
        ArrayBuffer vbo;
        ArrayBuffer ivbo;
        ShaderProgram program;
        FloatUniform timeUniform;

        private BulletBatch(VertexArray vao, ArrayBuffer vbo, ArrayBuffer ivbo, ShaderProgram program, FloatUniform timeUniform)
        {
            this.vao = vao;
            this.vbo = vbo;
            this.ivbo = ivbo;
            this.program = program;
            this.timeUniform = timeUniform;
        }

        public void Draw(RoArray<Position> positions, int length, float time)
        {
            program.Use();
            timeUniform.Set(time);
            ivbo.Bind();
            ivbo.BufferSubData(0, Marshal.SizeOf<Position>() * length, (Position[])positions);
            vao.Bind();
            Gl.DrawArraysInstanced(PrimitiveType.TriangleStrip, 0, 4, length);
            vao.Unbind();
        }

        public static BulletBatch CreateBulletBatch()
        {
            var program = CreateProgram();
            var timeUniform = program.GetFloatUniform("uTime");
            var vbo = Quad.GetVertexBuffer();
            var ivbo = ArrayBuffer.Create(Marshal.SizeOf<Position>() * 4096, BufferUsage.DynamicDraw);
            var vao = CreateVertexArray();
            return new BulletBatch(vao, vbo, ivbo, program, timeUniform);

            ShaderProgram CreateProgram()
            {
                using (var vertexShader = VertexShader.FromSource(Resources.BulletVertex))
                using (var fragShader = FragmentShader.FromSource(Resources.BulletFrag))
                {
                    return ShaderProgram.LinkShaders(vertexShader, fragShader);
                }
            }

            VertexArray CreateVertexArray()
            {
                var builder = new VertexArrayBuilder();
                builder.ChangeArrayBuffer(vbo);
                builder.SetAttribute(new Vec2Attribute(positionIndex), 0, 0);
                builder.ChangeArrayBuffer(ivbo);
                builder.SetAttribute(new Vec2Attribute(offsetIndex), 0, 0);
                builder.SetAttributeDivisor(offsetIndex, 1);
                return builder.Build();
            }
        }
    }
       
}
