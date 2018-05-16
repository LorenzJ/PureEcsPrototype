using GameGl.Core.Shaders;
using OpenGL;
using System.Numerics;

namespace GameGl.Core.Uniforms
{
    public struct Mat4Uniform : IUniform
    {
        public int Location { get; }

        public Mat4Uniform(int location)
        {
            Location = location;
        }

        public void Set(Matrix4x4 matrix)
        {
            unsafe
            {
                Gl.UniformMatrix4(Location, 1, false, &matrix.M11);
            }
        }
    }

    public static class Mat4UniformHelper
    {
        public static Mat4Uniform GetMat4Uniform(this ShaderProgram program, string name)
            => new Mat4Uniform(program.GetUniformLocation(name));
    }
}