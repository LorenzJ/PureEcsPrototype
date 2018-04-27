namespace GameGl.Core.Attributes
{
    public struct FloatAttribute : IAttribute
    {
        private uint index;
        public uint Index => index;

        public FloatAttribute(uint index)
        {
            this.index = index;
        }
    }
}
