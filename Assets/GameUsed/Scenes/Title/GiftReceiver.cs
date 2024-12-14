using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("Title/Gift Receiver")]
    public class GiftReceiver : MonoBehaviour
    {
        public bool IsReceiving { get; private set; }

        public async UniTask<object> Begin(object input)
        {
            IsReceiving = true;
            return null;
        }

        public async UniTask<object> Stop(object input)
        {
            IsReceiving = false;
            return null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsReceiving) Debug.Log("Gift received");
        }
    }
}