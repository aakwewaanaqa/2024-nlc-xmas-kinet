using System;

namespace GameUsed.Core
{
    public static class Exts
    {
        public static U Apply<T, U>(this T t, Func<T, U> f)
        {
            return f(t);
        }
    }
}