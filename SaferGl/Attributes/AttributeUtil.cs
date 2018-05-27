using OpenGL;

namespace SaferGl.Attributes
{
    public static class AttributeUtil
    {
        public static void SetAttributePointer<T>(this VertexArrayBinding @this, T attribute, int size, VertexAttribType attribType, bool normalized, int stride, int offset)
            where T : IAttribute => @this.SetAttributePointer(attribute.Index, size, attribType, normalized, stride, offset);

        public static void EnableAttribute<T>(this VertexArrayBinding @this, T attribute)
            where T : IAttribute => @this.EnableAttribute(attribute.Index);

        public static void DisableAttribute<T>(this VertexArrayBinding @this, T attribute)
            where T : IAttribute => @this.DisableAttribute(attribute.Index);

        public static void SetAttributePointer(this VertexArrayBinding @this, FloatAttribute attribute, int stride, int offset)
            => @this.SetAttributePointer(attribute, 1, VertexAttribType.Float, false, stride, offset);

        public static void SetAttributePointer(this VertexArrayBinding @this, Vec2Attribute attribute, int stride, int offset)
            => @this.SetAttributePointer(attribute, 2, VertexAttribType.Float, false, stride, offset);

        public static void SetAttributePointer(this VertexArrayBinding @this, Vec3Attribute attribute, int stride, int offset)
           => @this.SetAttributePointer(attribute, 3, VertexAttribType.Float, false, stride, offset);

        public static void SetAttributePointer(this VertexArrayBinding @this, Vec4Attribute attribute, int stride, int offset)
           => @this.SetAttributePointer(attribute, 4, VertexAttribType.Float, false, stride, offset);

        public static void SetAttributeDivisor<T>(this VertexArrayBinding @this, T attribute, uint divisor)
            where T : IAttribute => @this.SetAttributeDivisor(attribute.Index, divisor);
    }
}
