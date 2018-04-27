using Game.Components.Transform;
using GameGl.Core;
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
            return NewBulletBatch();

            BulletBatch NewBulletBatch()
            {
                var program = CreateProgram();
                var timeUniform = program.GetFloatUniform("uTime");
                var vbo = CreateVertexBuffer();
                var ivbo = ArrayBuffer.Create(Marshal.SizeOf<Position>() * 4096, BufferUsage.DynamicDraw);
                var vao = CreateVertexArray(vbo, ivbo);
                return new BulletBatch(vao, vbo, ivbo, program, timeUniform);
            }

            ShaderProgram CreateProgram()
            {
                using (var vertexShader = VertexShader.FromSource(Resources.BulletVertex))
                using (var fragShader = FragmentShader.FromSource(Resources.BulletFrag))
                {
                    return ShaderProgram.LinkShaders(vertexShader, fragShader);
                }
            }

            ArrayBuffer CreateVertexBuffer()
            {
                var vertices = new float[]
                {
                    0.5f, -0.5f, 0.5f, 0.5f, -0.5f, -0.5f, -0.5f, 0.5f
                };
                return ArrayBuffer.Create(vertices, sizeof(float) * vertices.Length);
            }

            uint CreateInstanceBuffer()
            {
                var ivbo = Gl.CreateBuffer();
                Gl.BindBuffer(BufferTarget.ArrayBuffer, ivbo);
                Gl.BufferData(BufferTarget.ArrayBuffer, (uint)Marshal.SizeOf<Position>() * 4096u, null, BufferUsage.DynamicDraw);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
                return ivbo;
            }

            uint CreateVertexArray(ArrayBuffer vbo, ArrayBuffer ivbo)
            {
                var builder = new VertexArrayBuilder();
                builder.ChangeArrayBuffer(vbo);
                builder.SetAttribute()
                Gl.BindVertexArray(vao);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                Gl.VertexAttribPointer(positionIndex, 2, VertexAttribType.Float, false, 0, IntPtr.Zero);
                Gl.EnableVertexAttribArray(positionIndex);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, ivbo);
                Gl.VertexAttribPointer(offsetIndex, 2, VertexAttribType.Float, false, 0, IntPtr.Zero);
                Gl.EnableVertexAttribArray(offsetIndex);
                Gl.VertexAttribDivisor(offsetIndex, 1);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
                Gl.BindVertexArray(0);
                
                return vao;
            }
        }
    }
       
}
