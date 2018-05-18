using SaferGl.Shaders;
using System;
using System.Linq.Expressions;

namespace SaferGl.Uniforms
{
    public static class UniformUtil
    {
        public static T GetUniform<T>(this ShaderProgram @this, string name)
            where T : IUniform
        {
            return UniformUtil<T>.GetUniform(@this, name);
        }
    }

    public static class UniformUtil<T>
        where T : IUniform
    {
        private static readonly Func<int, T> factory;

        static UniformUtil()
        {
            var constructor = typeof(T).GetConstructor(new Type[] { typeof(int) });
            var argument = Expression.Parameter(typeof(int));
            factory = Expression.Lambda<Func<int, T>>(
                Expression.New(constructor, argument), argument)
                .Compile();
        }

        public static T GetUniform(ShaderProgram program, string name)
            => factory(program.GetUniformLocation(name));
    }
}
