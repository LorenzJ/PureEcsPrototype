using System.Runtime.InteropServices;
using Game.Components.Transform;
using GameGl.Properties;
using OpenGL;
using SaferGl;
using SaferGl.Buffers;
using SaferGl.Shaders;
using SaferGl.Uniforms;
using SaferGl.Attributes;

namespace GameGl
{
    internal class ShipBatch
    {
        private ShaderProgram program;
        private ArrayBuffer ivbo;
        private VertexArray vao;
        private readonly FloatUniform timeUniform;
        private readonly FloatUniform scaleUniform;

        private const uint MAX_SHIPS = 256u;

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
            using (var vertexShader = ShaderFactory.FromSource<VertexShader>(Resources.BulletVertex))
            using (var fragmentShader = ShaderFactory.FromSource<FragmentShader>(Resources.ShipFrag))
            {
                program = ShaderProgram.LinkShaders(vertexShader, fragmentShader);
            }
            var timeUniform = program.GetUniform<FloatUniform>("uTime");
            var scaleUniform = program.GetUniform<FloatUniform>("uScale");

            var ivbo = BufferFactory.Create<ArrayBuffer>((uint)Marshal.SizeOf<Position>() * MAX_SHIPS, null, BufferUsage.StreamDraw);
            var vbo = Triangle.VertexBuffer;

            var vertexArray = VertexArray.Create();
            using (var vaoBinding = vertexArray.BindVertexArray())
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
                    Gl.VertexAttribDivisor(offsetAttribute.Index, 1u);
                    vaoBinding.EnableAttribute(offsetAttribute);
                }
            }

            return new ShipBatch(program, ivbo, vertexArray, timeUniform, scaleUniform);
        } 

        internal void Draw(Position[] positions, int count, float time)
        {
            using (var programBinding = program.Use())
            {
                programBinding.Set(scaleUniform, .1f);
                programBinding.Set(timeUniform, time);

                using (var ivboBinding = ivbo.BindBuffer())
                {
                    ivboBinding.Data((uint)Marshal.SizeOf<Position>() * MAX_SHIPS, null, BufferUsage.StreamDraw);
                    ivboBinding.SubData(0, (uint)Marshal.SizeOf<Position>() * (uint)count, positions);
                }

                using (var vaoBinding = vao.BindVertexArray())
                    vaoBinding.DrawArraysInstanced(PrimitiveType.Triangles, 0, 3, count);
            }
        }
    }
}