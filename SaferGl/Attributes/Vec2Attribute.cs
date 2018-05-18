namespace SaferGl.Attributes
{
    public struct Vec2Attribute : IAttribute
    {
        public uint Index { get; }

        public Vec2Attribute(uint index) : this()
        {
            Index = index;
        }
    }
}
