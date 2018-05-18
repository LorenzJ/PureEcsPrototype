using OpenGL;
using SaferGl.Shaders;
using System.Numerics;

namespace SaferGl.Uniforms
{
    public static class UniformProgramExtensions
    {
        public static void Set(this ProgramBinding _, FloatUniform uniform, float value)
            => Gl.Uniform1(uniform.Location, value);

        public static void Set(this ProgramBinding _, Vec2Uniform uniform, Vector2 vector)
            => Gl.Uniform2(uniform.Location, vector.X, vector.Y);

        public static void Set(this ProgramBinding _, Vec3Uniform uniform, Vector3 vector)
            => Gl.Uniform3(uniform.Location, vector.X, vector.Y, vector.Z);

        public static void Set(this ProgramBinding _, Vec4Uniform uniform, Vector4 vector)
            => Gl.Uniform4(uniform.Location, vector.X, vector.Y, vector.Z, vector.W);

        public static unsafe void Set(this ProgramBinding _, Mat4Uniform uniform, bool transpose, Matrix4x4 matrix) 
            => Gl.UniformMatrix4(uniform.Location, 1, transpose, &matrix.M11);
    }
}
