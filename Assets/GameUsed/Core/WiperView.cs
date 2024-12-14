using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GameUsed.Core
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    public class WiperView : MonoBehaviour
    {
        [SerializeField] private Image image;

        private CancellationTokenSource cts { get; set; } = new();

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private static Func<float, float, bool> FloatsEquals => ((a, b) => Math.Abs(a - b) < 0.0001f);

        public async UniTask<object> Show(object input, CancellationToken ct = default)
        {
            cts = cts.Link(ct, out var inner);

            const float TO    = 1f;
            for (var t = image.fillAmount;
                 FloatsEquals(t, TO);
                 t += (TO - t) * Time.deltaTime)
            {
                if (inner.IsCancellationRequested) break;
                image.fillAmount = t;
                await UniTask.Yield();
            }

            return null;
        }

        public async UniTask<object> Hide(object input, CancellationToken ct = default)
        {
            cts = cts.Link(ct, out var inner);

            const float TO = 0f;
            for (var t = image.fillAmount;
                 FloatsEquals(t, TO);
                 t += (TO - t) * Time.deltaTime)
            {
                if (inner.IsCancellationRequested) break;
                image.fillAmount = t;
                await UniTask.Yield();
            }

            return null;
        }
    }
}