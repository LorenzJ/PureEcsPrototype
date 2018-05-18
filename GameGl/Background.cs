using GameGl.Properties;
using OpenGL;
using SaferGl;
using SaferGl.Attributes;
using SaferGl.Buffers;
using SaferGl.Shaders;
using SaferGl.Uniforms;
using System;
using System.Numerics;

namespace GameGl
{
    internal class Background
    {
        private ShaderProgram program;
        private FloatUniform timeUniform;
        private Mat4Uniform viewProjectionUniform;
        private VertexArray vao;

        internal static Background Create()
        {
            Background background = new Background();
            using (var vertexShader = ShaderFactory<VertexShader>.FromSource(Resources.BackgroundVertex))
            using (var fragmentShader = ShaderFactory<FragmentShader>.FromSource(Resources.BackgroundFrag))
            {
                background.program = ShaderProgram.LinkShaders(vertexShader, fragmentShader);
            }
            background.timeUniform = background.program.GetUniform<FloatUniform>("uTime");
            background.viewProjectionUniform = background.program.GetUniform<Mat4Uniform>("uViewProjection");

            var vao = VertexArray.Create();
            using (var vaoBinding = vao.BindVertexArray())
            {
                using (var vboBinding = Quad.VertexBuffer.BindBuffer())
                {
                    var positionAttribute = new Vec2Attribute(0u);
                    vaoBinding.SetAttributePointer(positionAttribute, 0, 0);
                    vaoBinding.EnableAttribute(positionAttribute);
                }
            }
            background.vao = vao;

            return background;
        }

        internal void Draw(float time)
        {
            var rotation = Matrix4x4.CreateFromYawPitchRoll(0f, 1.6f, 0f);
            var translation = Matrix4x4.CreateTranslation(new Vector3((float)Math.Sin(time*.2)*.5f, -.3f, -.8f));
            var projection = Matrix4x4.CreatePerspective(1f, 1f, .1f, 100f);

            using (var programBinding = program.Use())
            {
                programBinding.Set(viewProjectionUniform, false, rotation * translation * projection);
                programBinding.Set(timeUniform, time);
                using (var vaoBinding = vao.BindVertexArray())
                {
                    Gl.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
                }
            }
        }
    }
}