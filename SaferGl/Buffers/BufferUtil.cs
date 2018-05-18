using OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace SaferGl.Buffers
{
    public static class BufferUtil
    {
        public static BufferBinding<T> BindBuffer<T>(this T @this)
            where T : IBuffer => new BufferBinding<T>(@this);

        public static void Delete<T>(this T @this)
            where T : IBuffer => Gl.DeleteBuffers(new uint[] { @this.Handle });

        public static void Delete<T>(this IEnumerable<T> buffers)
            where T : IBuffer => Gl.DeleteBuffers(buffers.Select(b => b.Handle).ToArray());
    }
}
