using GameGl.Core.Shaders;
using OpenGL;

namespace GameGl.Core.Uniforms
{
    public struct FloatUniform : IUniform
    {
        public int Location { get; }

        internal FloatUniform(int location)
        {
            Location = location;
        }

        public void Set(float value)
        {
            Gl.Uniform1(Location, value);
        }
    }

    public static class FloatUniformUtil
    {
        public static FloatUniform GetFloatUniform(this ShaderProgram program, string name)
            => new FloatUniform(program.GetUniformLocation(name));
    }


}
