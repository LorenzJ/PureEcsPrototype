namespace SaferGl.Uniforms
{
    public struct Vec4Uniform : IUniform
    {
        public int Location { get; }

        public Vec4Uniform(int location) : this()
        {
            Location = location;
        }
    }
}