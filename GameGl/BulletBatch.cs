using Game.Components.Transform;
using GameGl.Core;
using GameGl.Core.Attributes;
using GameGl.Core.Buffers;
using GameGl.Core.Shaders;
using GameGl.Core.Uniforms;
using GameGl.Properties;
using OpenGL;
using System.Runtime.InteropServices;
using TinyEcs;

namespace GameGl
{
    internal class BulletBatch
    {
        const int positionIndex = 0;
        const int offsetIndex = 1;

        VertexArray vao;
        ArrayBuffer ivbo;
        ShaderProgram program;
        FloatUniform timeUniform;
        FloatUniform scaleUniform;

        private BulletBatch(VertexArray vao, ArrayBuffer ivbo, ShaderProgram program, FloatUniform timeUniform, FloatUniform scaleUniform)
        {
            this.vao = vao;
            this.ivbo = ivbo;
            this.program = program;
            this.timeUniform = timeUniform;
            this.scaleUniform = scaleUniform;
        }

        public void Draw(Position[] positions, int length, float time)
        {
            program.Use();
            timeUniform.Set(time);
            scaleUniform.Set(.05f);
            ivbo.Bind();
            ivbo.BufferSubData(0, Marshal.SizeOf<Position>() * length, positions);
            ivbo.Unbind();
            vao.Bind();
            Gl.DrawArraysInstanced(PrimitiveType.TriangleStrip, 0, 4, length);
            vao.Unbind();
        }

        public static BulletBatch Create()
        {
            var program = CreateProgram();
            var timeUniform = program.GetFloatUniform("uTime");
            var scaleUniform = program.GetFloatUniform("uScale");
            var vbo = Quad.VertexBuffer;
            var ivbo = ArrayBuffer.Create(Marshal.SizeOf<Position>() * 4000, BufferUsage.DynamicDraw);
            var vao = CreateVertexArray();
            return new BulletBatch(vao, ivbo, program, timeUniform, scaleUniform);

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
