using Game.Components.Transform;
using GameGl.Properties;
using OpenGL;
using SaferGl;
using SaferGl.Attributes;
using SaferGl.Buffers;
using SaferGl.Shaders;
using SaferGl.Uniforms;
using System.Runtime.InteropServices;

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
            using (var programBinding = program.Use())
            {
                programBinding.Set(timeUniform, time);
                programBinding.Set(scaleUniform, 0.05f);
                using (var ivboBinding = ivbo.BindBuffer())
                {
                    ivboBinding.SubData(0, (uint)Marshal.SizeOf<Position>() * (uint)length, positions);
                }
                using (var vaoBinding = vao.BindVertexArray())
                {
                    Gl.DrawArraysInstanced(PrimitiveType.TriangleStrip, 0, 4, length);
                }
            }
        }

        public static BulletBatch Create()
        {
            var program = CreateProgram();
            var timeUniform = program.GetUniform<FloatUniform>("uTime");
            var scaleUniform = program.GetUniform<FloatUniform>("uScale");
            var vbo = Quad.VertexBuffer;
            var ivbo = BufferFactory.Create<ArrayBuffer>((uint)Marshal.SizeOf<Position>() * 4000u, null, BufferUsage.DynamicDraw);
            var vao = CreateVertexArray();
            return new BulletBatch(vao, ivbo, program, timeUniform, scaleUniform);

            ShaderProgram CreateProgram()
            {
                using (var vertexShader = ShaderFactory<VertexShader>.FromSource(Resources.BulletVertex))
                using (var fragShader = ShaderFactory<FragmentShader>.FromSource(Resources.BulletFrag))
                {
                    return ShaderProgram.LinkShaders(vertexShader, fragShader);
                }
            }

            VertexArray CreateVertexArray()
            {
                var newVao = VertexArray.Create();
                using (var vaoBinding = newVao.BindVertexArray())
                {
                    using (var vboBinding = vbo.BindBuffer())
                    {
                        var positionAttribute = new Vec2Attribute(0u);
                        vaoBinding.SetAttributePointer(positionAttribute, 0, 0);
                        vaoBinding.EnableAttribute(positionAttribute);
                    }
                    using (var ivboBinding = ivbo.BindBuffer())
                    {
                        var offsetAttribute = new Vec2Attribute(1u);
                        vaoBinding.SetAttributePointer(offsetAttribute, 0, 0);
                        vaoBinding.EnableAttribute(offsetAttribute);
                        Gl.VertexAttribDivisor(1, 1);
                    }
                }
                return newVao;
            }
        }
    }
       
}
