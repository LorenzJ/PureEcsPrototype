namespace SaferGl.Attributes
{
    public struct Vec4Attribute : IAttribute
    {
        public uint Index { get; }

        public Vec4Attribute(uint index) : this()
        {
            Index = index;
        }
    }
}
