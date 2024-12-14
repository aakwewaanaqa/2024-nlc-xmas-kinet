using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

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
                catch { return new PipeReturn(null, onNg); }
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
                catch (Exception ex) { return new PipeReturn(ex, null); }
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
                catch { return new PipeReturn(null, f.RetryThen(onOk)); }
            };
        }

        public static CancellationTokenSource Link(
            this CancellationTokenSource cts,
            CancellationToken            outer,
            out CancellationToken        inner)
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            outer.Register(() => cts.Cancel());
            inner = cts.Token;
            return cts;
        }

        public static async UniTask LerpTo(
            this float        f,
            float             t,
            float             speed  = 1f,
            Action<float>     update = null,
            CancellationToken ct     = default)
        {
            for (; !f.Equals(t); f += (t - f) * speed * Time.deltaTime)
            {
                if (ct.IsCancellationRequested) break;
                update?.Invoke(f);
                await UniTask.Yield();
            }
        }

        public static async UniTask Loop(
            this float        interval,
            Action<int>       update = null,
            CancellationToken ct     = default)
        {
            var index = 0;
            while (!ct.IsCancellationRequested)
            {
                update?.Invoke(index++);
                await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: ct);
            }
        }

        private static bool Equals(this float f, float t)
        {
            return Math.Abs(f - t) < 0.01f;
        }
    }
}