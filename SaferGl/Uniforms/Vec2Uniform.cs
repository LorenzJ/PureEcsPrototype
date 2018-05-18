namespace SaferGl.Uniforms
{
    public struct Vec2Uniform : IUniform
    {
        public int Location { get; }

        public Vec2Uniform(int location)
        {
            Location = location;
        }
    }
}
