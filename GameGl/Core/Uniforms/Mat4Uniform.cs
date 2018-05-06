using OpenGL;
using System.Numerics;

namespace GameGl.Core.Uniforms
{
    public struct Mat4Uniform : IUniform
    {
        private int location;

        public int Location => location;

        public Mat4Uniform(int location)
        {
            this.location = location;
        }

        public void Set(Matrix4x4 matrix)
        {
            unsafe
            {
                Gl.UniformMatrix4(location, 1, false, &matrix.M11);
            }
        }
    }
}