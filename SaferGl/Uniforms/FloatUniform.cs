namespace SaferGl.Uniforms
{
    public struct FloatUniform : IUniform
    {
        public int Location { get; }

        public FloatUniform(int location)
        {
            Location = location;
        }
    }
}
