using System;

namespace GameUsed.Core
{
    public static class Exts
    {
        public static U Apply<T, U>(this T t, Func<T, U> f)
        {
            return f(t);
        }

        public static PipeFunc Then(this PipeFunc f, PipeFunc onOk, PipeFunc onNg)
        {
            return async () =>
            {
                try
                {
                    var r = await f();
                    if (r.IsFaulty) throw r.Ex;
                    return new PipeReturn(null, onOk);
                }
                catch
                {
                    return new PipeReturn(null, onNg);
                }
            };
        }

        public static PipeFunc Then(this PipeFunc f, PipeFunc onOk)
        {
            return async () =>
            {
                try
                {
                    var r = await f();
                    if (r.IsFaulty) throw r.Ex;
                    return new PipeReturn(null, onOk);
                }
                catch (Exception ex)
                {
                    return new PipeReturn(ex, null);
                }
            };
        }

        public static PipeFunc RetryThen(this PipeFunc f, PipeFunc onOk)
        {
            return async () =>
            {
                try
                {
                    var r = await f();
                    if (r.IsFaulty) throw r.Ex;
                    return new PipeReturn(null, onOk);
                }
                catch
                {
                    return new PipeReturn(null, f.RetryThen(onOk));
                }
            };
        }
    }
}