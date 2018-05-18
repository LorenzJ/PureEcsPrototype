using System;
using System.Linq.Expressions;

namespace SaferGl
{
    public static class OglObjectFactoryFactory
    {
        public static Func<T> Create<T>(Func<uint> generator)
        {
            var constructor = typeof(T).GetConstructor(new Type[] { typeof(uint) });
            return Expression.Lambda<Func<T>>(
                Expression.New(constructor, Expression.Call(generator.Method)))
            .Compile();
        }
    }
}
