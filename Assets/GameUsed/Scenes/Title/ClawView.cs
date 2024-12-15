using System.Threading;
using System.Threading.Tasks;
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

        public async UniTask<object> Show(object input)
        {
            cts = cts.Link(default, out var inner);
            gameObject.SetActive(true);
            await group.alpha.LerpTo(1f, 15f, f =>
            {
                group.alpha = f;
            }, inner);
            group.interactable = true;

            goLeft.onClick.RemoveAllListeners();
            goLeft.onClick.AddListener(() => claw.Shift(Claw.Direction.Left));

            goRight.onClick.RemoveAllListeners();
            goRight.onClick.AddListener(() => claw.Shift(Claw.Direction.Right));

            grab.onClick.RemoveAllListeners();
            grab.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                var gift = (Gift) await claw.Grab();
                if (gift.IsObject()) Main.Pipeline.blessing = gift.blessing;
            }));

            return null;
        }

        public async UniTask<object> Hide(object input)
        {
            cts = cts.Link(default, out var inner);

            group.interactable = false;
            await group.alpha.LerpTo(0f, 3f, f =>
            {
                group.alpha = f;
            }, ct: inner);
            gameObject.SetActive(true);

            return null;
        }
    }
}