namespace GameGl.Core.Attributes
{
    public struct FloatAttribute : IAttribute
    {
        public uint Index { get; }

        public FloatAttribute(uint index)
        {
            Index = index;
        }
    }
}
