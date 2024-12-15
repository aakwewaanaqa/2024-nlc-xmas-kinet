using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameUsed.Core
{
    public static class Exts
    {
        public static bool IsObject(this object o)
        {
            return o != null && !o.Equals(null);
        }

        public static U Apply<T, U>(this T t, Func<T, U> f)
        {
            return f(t);
        }

        public static void Let<T>(this T t, Action<T> f)
        {
            f(t);
        }

        public static Pipe Then(this Pipe f, Pipe onOk, Pipe onNg)
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
                    Debug.LogException(ex);
                    return new PipeReturn(null, onNg);
                }
            };
        }

        public static Pipe Then(this Pipe f, Pipe onOk)
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
                    Debug.LogException(ex);
                    return new PipeReturn(ex);
                }
            };
        }

        public static Pipe RetryThen(this Pipe f, Pipe onOk)
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
                    Debug.LogException(ex);
                    return new PipeReturn(null, f.RetryThen(onOk));
                }
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
            for (; !f.IsCloseTo(t); f += (t - f) * speed * Time.deltaTime)
            {
                if (ct.IsCancellationRequested) break;
                update?.Invoke(f);
                await UniTask.Yield();
            }

            update?.Invoke(t);
        }

        public static async UniTask MoveTo(
            this Vector3      f,
            Vector3           t,
            float             speed  = 1f,
            Action<Vector3>   update = null,
            CancellationToken ct     = default)
        {

            for (; !f.IsCloseTo(t); f += (t - f) * speed * Time.deltaTime)
            {
                var distance = (t - f).sqrMagnitude;
                var delta = (t - f).normalized * speed * Time.deltaTime;
                if (distance < delta.sqrMagnitude) f = t;
                else f += delta;
                if (ct.IsCancellationRequested) break;
                update?.Invoke(f);
                await UniTask.Yield();
            }

            update?.Invoke(t);
        }

        public static async UniTask LerpTo(
            this Vector3      f,
            Vector3           t,
            float             speed  = 1f,
            Action<Vector3>   update = null,
            CancellationToken ct     = default)
        {
            for (; !f.IsCloseTo(t); f += (t - f) * speed * Time.deltaTime)
            {
                if (ct.IsCancellationRequested) break;
                update?.Invoke(f);
                await UniTask.Yield();
            }

            update?.Invoke(t);
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

        public static async UniTask<PipeReturn> Engage(this Pipe entry)
        {
            var r = await entry();
            if (r.IsFaulty) Debug.LogError(r.Ex);
            while (!r.IsEnd)
            {
                r = await r.Continue();
                if (r.IsFaulty) Debug.LogError(r.Ex);
            }

            return r;
        }

        private static bool IsCloseTo(this float f, float t)
        {
            return Math.Abs(f - t) < 0.01f;
        }

        private static bool IsCloseTo(this Vector3 f, Vector3 t)
        {
            return (f - t).sqrMagnitude <= float.Epsilon;
        }
    }
}