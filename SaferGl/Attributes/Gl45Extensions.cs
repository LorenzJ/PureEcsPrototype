using OpenGL;

namespace SaferGl.Attributes.Gl45
{
    public static class Gl45Extensions
    {
        public static void EnableAttribute(this VertexArray @this, uint index)
            => Gl.EnableVertexArrayAttrib(@this.Handle, index);

        public static void DisableAttribute(this VertexArray @this, uint index)
            => Gl.DisableVertexArrayAttrib(@this.Handle, index);

        public static void EnableAttribute<T>(this VertexArray @this, T attribute)
            where T : IAttribute => @this.EnableAttribute(attribute.Index);

        public static void DisableAttribute<T>(this VertexArray @this, T attribute)
            where T : IAttribute => @this.DisableAttribute(attribute.Index);
    }
}
