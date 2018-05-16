namespace GameGl.Core.Attributes
{
    public struct Vec2Attribute : IAttribute
    {
        public uint Index { get; }

        public Vec2Attribute(uint index)
        {
            Index = index;
        }
    }
}
