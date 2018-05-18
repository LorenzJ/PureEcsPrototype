namespace SaferGl.Attributes
{
    public struct FloatAttribute : IAttribute
    {
        public uint Index { get; }

        public FloatAttribute(uint index) : this()
        {
            Index = index;
        }
    }
}
