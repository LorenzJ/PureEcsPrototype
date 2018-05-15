using System.Runtime.InteropServices;
using Game.Components.Transform;
using GameGl.Core;
using GameGl.Core.Attributes;
using GameGl.Core.Buffers;
using GameGl.Core.Shaders;
using GameGl.Core.Uniforms;
using GameGl.Properties;
using OpenGL;

namespace GameGl
{
    internal class ShipBatch
    {
        private ShaderProgram program;
        private ArrayBuffer ivbo;
        private VertexArray vao;
        private FloatUniform timeUniform;
        private FloatUniform scaleUniform;

        public ShipBatch(ShaderProgram program, ArrayBuffer ivbo, VertexArray vao, FloatUniform timeUniform, FloatUniform scaleUniform)
        {
            this.program = program;
            this.ivbo = ivbo;
            this.vao = vao;
            this.timeUniform = timeUniform;
            this.scaleUniform = scaleUniform;
        }

        internal static ShipBatch Create()
        {
            ShaderProgram program;
            using (var vertexShader = VertexShader.FromSource(Resources.BulletVertex))
            using (var fragmentShader = FragmentShader.FromSource(Resources.ShipFrag))
            {
                program = ShaderProgram.LinkShaders(vertexShader, fragmentShader);
            }
            var timeUniform = program.GetFloatUniform("uTime");
            var scaleUniform = program.GetFloatUniform("uScale");

            var ivbo = ArrayBuffer.Create(Marshal.SizeOf<Position>() * 256, BufferUsage.DynamicDraw);
            var vbo = Triangle.VertexBuffer;

            var builder = new VertexArrayBuilder();
            builder.ChangeArrayBuffer(vbo);
            builder.SetAttribute(new Vec2Attribute(0), 0, 0);
            builder.ChangeArrayBuffer(ivbo);
            builder.SetAttribute(new Vec2Attribute(1), 0, 0);
            builder.SetAttributeDivisor(1, 1);
            var vao = builder.Build();

            return new ShipBatch(program, ivbo, vao, timeUniform, scaleUniform);
        } 

        internal void Draw(Position[] positions, int count, float time)
        {
            program.Use();
            scaleUniform.Set(.1f);
            timeUniform.Set(time);
            ivbo.Bind();
            ivbo.BufferSubData(0, Marshal.SizeOf<Position>() * count, positions);
            ivbo.Unbind();
            vao.Bind();
            Gl.DrawArraysInstanced(PrimitiveType.Triangles, 0, 3, count);
            vao.Unbind();
        }
    }
}