using OpenGL;
using System.Text;

namespace SaferGl.Shaders
{
    public static class ShaderUtil
    {
        public static void Source<T>(this T @this, params string[] src)
            where T : IShader => Gl.ShaderSource(@this.Handle, src);

        public static void Compile<T>(this T @this)
            where T : IShader => Gl.CompileShader(@this.Handle);

        public static int GetCompileStatus<T>(this T @this)
            where T : IShader
        {
            Gl.GetShader(@this.Handle, ShaderParameterName.CompileStatus, out var status);
            return status;
        }

        public static int GetInfoLogLength<T>(this T @this)
            where T : IShader
        {
            Gl.GetShader(@this.Handle, ShaderParameterName.InfoLogLength, out var length);
            return length;
        }

        public static string GetInfoLog<T>(this T @this)
            where T : IShader
        {
            var length = GetInfoLogLength(@this);
            var infoLog = new StringBuilder(length);
            Gl.GetShaderInfoLog(@this.Handle, length, out var _, infoLog);
            return infoLog.ToString();
        }
        public static void Delete<T>(this T @this)
            where T : IShader => Gl.DeleteShader(@this.Handle);
    }

}
