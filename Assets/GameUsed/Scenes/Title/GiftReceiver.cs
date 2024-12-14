using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("Title/Gift Receiver")]
    public class GiftReceiver : MonoBehaviour
    {
        private bool isReceiving { get; set; } // 表示正在等待接收禮物
        private bool isReceived  { get; set; } // 表示收到禮物了

        public async UniTask<object> Begin(object input)
        {
            isReceiving = true;
            return null;
        }

        public async UniTask<object> Stop(object input)
        {
            isReceiving = false;
            return null;
        }

        public async UniTask<object> WaitForGiftReceived(object input)
        {
            await UniTask.WaitUntil(() => isReceived);
            return null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isReceiving) return;

            Debug.Log("gift received");
            isReceived = true;
        }
    }
}