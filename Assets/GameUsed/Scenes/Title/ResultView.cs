using System.Threading;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using TMPro;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("GameUsed/Scenes/Title/Result View")]
    public class ResultView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TextMeshProUGUI blessing;
        [SerializeField] private KButton skip;

        private static CancellationTokenSource cts { get; set; } = new();
        
        public async UniTask<object> Show(object input, CancellationToken ct = default)
        {
            cts = cts.Link(ct, out var inner);
            
            gameObject.SetActive(true);
            
            await group.alpha.LerpTo(1f, 5f, a => group.alpha = a, ct: inner);
            
            var isWaiting = true;
            blessing.text = (string) input;            
            skip.onClick.RemoveAllListeners();
            skip.onClick.AddListener(() =>
            {
                isWaiting = false;
            });
            await UniTask.WaitWhile(() => isWaiting, cancellationToken: inner);

            return null;
        }
        
        public async UniTask<object> Hide(object input, CancellationToken ct = default)
        {
            cts = cts.Link(ct, out var inner);

            await group.alpha.LerpTo(0f, 5f, a => group.alpha = a, ct: inner);
            
            gameObject.SetActive(false);
            
            return null;
        }
    }
}