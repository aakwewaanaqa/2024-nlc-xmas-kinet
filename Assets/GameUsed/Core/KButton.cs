using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameUsed.Core
{
    // 為了讓在測試時可以用滑鼠互動，又能讓Kinect互動，製作的按鈕
    public class KButton :
        MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler
    {
        [Tooltip("是否重複執行 OnClick")]
        [SerializeField] public bool isRepeatedly;

        [Tooltip("按下時執行的事件")]
        [SerializeField] public UnityEvent onClick;

        private CancellationTokenSource cts { get; set; } = new();

        public void OnPointerDown(PointerEventData eventData)
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            if (isRepeatedly) Loop().Forget();
            else onClick?.Invoke();
        }

        private async UniTask Loop()
        {
            while (!cts.IsCancellationRequested)
            {
                await UniTask.WaitForFixedUpdate();
                onClick?.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            cts?.Cancel();
        }

        private void OnDestroy()
        {
            cts?.Cancel();
            cts?.Dispose();
        }
    }
}