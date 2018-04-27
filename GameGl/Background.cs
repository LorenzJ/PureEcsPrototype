using GameGl.Core;
using GameGl.Core.Attributes;
using GameGl.Core.Shaders;
using GameGl.Core.Uniforms;
using GameGl.Properties;
using OpenGL;

namespace GameGl
{
    internal class Background
    {
        private ShaderProgram program;
        private FloatUniform timeUniform;
        private VertexArray vao;

        internal static Background CreateBackground()
        {
            Background background = new Background();
            using (var vertexShader = VertexShader.FromSource(Resources.BackgroundVertex))
            using (var fragmentShader = FragmentShader.FromSource(Resources.BackgroundFrag))
            {
                background.program = ShaderProgram.LinkShaders(vertexShader, fragmentShader);
            }
            background.timeUniform = background.program.GetFloatUniform("uTime");

            var builder = new VertexArrayBuilder();
            builder.ChangeArrayBuffer(Quad.GetVertexBuffer());
            builder.SetAttribute(new Vec2Attribute(0), 0, 0);
            background.vao = builder.Build();

            return background;
        }

        internal void Draw(float time)
        {
            program.Use();
            timeUniform.Set(time);
            vao.Bind();
            Gl.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            vao.Unbind();
        }
    }
}