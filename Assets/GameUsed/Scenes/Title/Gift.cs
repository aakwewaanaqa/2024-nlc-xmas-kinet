using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;
using static UnityEngine.Random;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("Title/Gift")]
    public class Gift : MonoBehaviour
    {
        [NonSerialized] public string qrCode;
        [NonSerialized] public string blessing;

        private CancellationTokenSource cts { get; } = new();

        public void PreventFromDestroy()
        {
            cts.Cancel();
        }

        private async UniTask Start()
        {
            GetComponent<Rigidbody>().mass = Range(1f, 3f);
            var trans = transform;
            0f.LerpTo(1f, 15f, t =>
            {
                trans.localScale = Vector3.one * t; // 禮物放大
            }, cts.Token).Forget();                 // 不要等
            await UniTask.Delay(25000, cancellationToken: cts.Token);
            1f.LerpTo(0f, 45f, t =>
            {
                trans.localScale = Vector3.one * t; // 禮物縮小
            }, cts.Token).Forget();                 // 不要等
            await UniTask.Delay(1000, cancellationToken: cts.Token);
            if (!cts.Token.IsCancellationRequested) Destroy(gameObject);
        }
    }
}