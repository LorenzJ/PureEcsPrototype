using GameGl.Core;
using GameGl.Core.Attributes;
using GameGl.Core.Shaders;
using GameGl.Core.Uniforms;
using GameGl.Properties;
using OpenGL;
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
            using (var vertexShader = VertexShader.FromSource(Resources.BackgroundVertex))
            using (var fragmentShader = FragmentShader.FromSource(Resources.BackgroundFrag))
            {
                background.program = ShaderProgram.LinkShaders(vertexShader, fragmentShader);
            }
            background.timeUniform = background.program.GetFloatUniform("uTime");
            background.viewProjectionUniform = background.program.GetMat4Uniform("uViewProjection");

            var builder = new VertexArrayBuilder();
            builder.ChangeArrayBuffer(Quad.VertexBuffer);
            builder.SetAttribute(new Vec2Attribute(0), 0, 0);
            background.vao = builder.Build();

            return background;
        }

        internal void Draw(float time)
        {
            program.Use();
            var rotation = Matrix4x4.CreateFromYawPitchRoll(0f, 1.6f, 0f);
            var translation = Matrix4x4.CreateTranslation(new Vector3((float)Math.Sin(time*.2)*.5f, -.3f, -.8f));
            var projection = Matrix4x4.CreatePerspective(1f, 1f, .1f, 100f);

            viewProjectionUniform.Set(rotation * translation * projection);
            //viewProjectionUniform.Set(Matrix4x4.CreateRotationZ(time) * Matrix4x4.CreateScale(1.5f));
            timeUniform.Set(time);
            vao.Bind();
            Gl.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            vao.Unbind();
        }
    }
}