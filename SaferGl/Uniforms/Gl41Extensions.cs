using OpenGL;
using SaferGl.Shaders;
using System.Numerics;

namespace SaferGl.Uniforms
{
    namespace Gl41
    {
        public static class Gl41Extensions
        {
            public static void Set(this ShaderProgram @this, FloatUniform uniform, float value)
                => Gl.ProgramUniform1(@this.Handle, uniform.Location, value);

            public static void Set(this ShaderProgram @this, Vec2Uniform uniform, Vector2 vector)
                => Gl.ProgramUniform2(@this.Handle, uniform.Location, vector.X, vector.Y);

            public static void Set(this ShaderProgram @this, Vec3Uniform uniform, Vector3 vector)
                => Gl.ProgramUniform3(@this.Handle, uniform.Location, vector.X, vector.Y, vector.Z);

            public static void Set(this ShaderProgram @this, Vec4Uniform uniform, Vector4 vector)
                => Gl.ProgramUniform4(@this.Handle, uniform.Location, vector.X, vector.Y, vector.Z, vector.W);

            public static unsafe void Set(this ShaderProgram @this, Mat4Uniform uniform, bool transpose, Matrix4x4 matrix)
                => Gl.ProgramUniformMatrix4(@this.Handle, uniform.Location, 1, transpose, &matrix.M11);
        }
    }
}
