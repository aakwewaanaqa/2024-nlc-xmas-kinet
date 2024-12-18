using System.Threading;
using System.Threading.Tasks;
using Codice.Client.Common.Connection;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("Title/Claw View")]
    public class ClawView : MonoBehaviour
    {
        [SerializeField] private Claw        claw;
        [SerializeField] private KButton     goLeft;
        [SerializeField] private KButton     goRight;
        [SerializeField] private KButton     grab;
        [SerializeField] private CanvasGroup group;

        private CancellationTokenSource cts { get; set; }

        private bool IsInteractable
        {
            set
            {
                var ct = cts.Token;
                goLeft.IsInteractable = value;
                goRight.IsInteractable = value;
                grab.IsInteractable = value;
                var t = value ? 1f : 0f;
                group.alpha.LerpTo(t, 15f, f =>
                {
                    group.alpha = f;
                }, ct).Forget();
            }
        }

        public async UniTask<object> Show(object input)
        {
            cts = cts.Link(default, out var inner);
            
            gameObject.SetActive(true);
            
            IsInteractable = true;
            await group.alpha.LerpTo(1f, 15f, f =>
            {
                group.alpha = f;
            }, inner);

            goLeft.onClick.RemoveAllListeners();
            goLeft.onClick.AddListener(() => claw.Shift(Claw.Direction.Left));

            goRight.onClick.RemoveAllListeners();
            goRight.onClick.AddListener(() => claw.Shift(Claw.Direction.Right));

            grab.onClick.RemoveAllListeners();
            grab.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                IsInteractable = false;
                
                var gift = (Gift) await claw.Grab();
                if (gift.IsObject())
                {
                    Main.Pipeline.blessing = gift.blessing;
                    return;
                }
                
                IsInteractable = true;
            }));

            return null;
        }

        public async UniTask<object> Hide(object input)
        {
            cts = cts.Link(default, out var inner);

            IsInteractable = false;

            return null;
        }
    }
}