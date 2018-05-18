namespace SaferGl.Attributes
{
    public struct Vec3Attribute : IAttribute
    {
        public uint Index { get; }

        public Vec3Attribute(uint index) : this()
        {
            Index = index;
        }
    }
}
