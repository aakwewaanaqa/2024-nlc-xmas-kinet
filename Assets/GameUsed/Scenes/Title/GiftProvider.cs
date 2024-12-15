using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using static UnityEngine.Random;
using Random = UnityEngine.Random;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("Title/Gift Provider")]
    public class GiftProvider : MonoBehaviour
    {
        [SerializeField] private GameUsed.Core.Bounds bounds;
        [SerializeField] private TextAsset            blessingData;
        [SerializeField] private GameObject           gift;

        private static string[]                blessings { get; set; }
        private static CancellationTokenSource cts       { get; set; } = new();

        private void Awake()
        {
            blessings =
                JsonConvert.DeserializeObject<string[]>(blessingData.text);
        }

        public async UniTask<object> Begin(object input)
        {
            cts = cts.Link(default, out var inner);
            await 1f.Loop(i =>
            {
                var position = bounds.GetRandomPosition();
                Instantiate(gift, position, Random.rotation)
                   .GetComponent<Gift>().Let(g =>
                    {
                        g.blessing = blessings[Range(0, blessings.Length)];
                    });
            }, inner);
            return null;
        }

        public async UniTask<object> Stop(object input)
        {
            cts.Cancel();
            await UniTask.Yield();
            return null;
        }
    }
}