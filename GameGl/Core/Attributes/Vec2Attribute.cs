namespace GameGl.Core.Attributes
{
    public struct Vec2Attribute : IAttribute
    {
        private uint index;
        public uint Index => index;

        public Vec2Attribute(uint index)
        {
            this.index = index;
        }
    }
}
