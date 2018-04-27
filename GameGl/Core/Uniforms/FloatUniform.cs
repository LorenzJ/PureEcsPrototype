using OpenGL;

namespace GameGl.Core.Uniforms
{
    public struct FloatUniform : IUniform
    {
        private int location;
        public int Location => location;

        internal FloatUniform(int location)
        {
            this.location = location;
        }

        public void Set(float value)
        {
            Gl.Uniform1(location, value);
        }
    }
}
