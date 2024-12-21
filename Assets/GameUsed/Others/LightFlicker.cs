using System;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Random;

namespace GameUsed.Others
{
    [AddComponentMenu("GameUsed/Others/Light Flicker")]
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private float scale;

        private void Start()
        {
            UniTask.Create(Loop).Forget();
        }

        private async UniTask Loop()
        {
            var ct = this.GetCancellationTokenOnDestroy();
            var light = GetComponent<Light>();
            var original = light.intensity;
            if (!light.IsObject()) return;
            while (true)
            {
                ct.ThrowIfCancellationRequested();
                var y = noise.snoise(new float2(Time.time, 0));
                y = math.remap(-1, 1, 0, 1, y);
                light.intensity = original + y * scale;
                await UniTask.Yield();
            }
        }
    }
}