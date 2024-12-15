using System;
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
        [NonSerialized] public string bendicion;

        private async UniTask Start()
        {
            GetComponent<Rigidbody>().mass = Range(1f, 3f);
            var trans = transform;
            0f.LerpTo(1f, 15f, t =>
            {
                trans.localScale = Vector3.one * t;
            }).Forget();
            await UniTask.Delay(25000);
            1f.LerpTo(0f, 45f, t =>
            {
                trans.localScale = Vector3.one * t;
            }).Forget();
            await UniTask.Delay(1000);
            Destroy(gameObject);
        }
    }
}