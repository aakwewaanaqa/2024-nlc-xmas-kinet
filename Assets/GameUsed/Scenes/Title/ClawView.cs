using System.Threading;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("Title/Claw View")]
    public class ClawView : MonoBehaviour
    {
        [SerializeField] private Claw  claw;
        [SerializeField] private Image goLeft;
        [SerializeField] private Image goRight;
        [SerializeField] private Image grab;
        [SerializeField] private CanvasGroup group;

        private CancellationTokenSource cts { get; set; }

        public async UniTask<object> Show(object input)
        {
            cts = cts.Link(default, out var inner);
            await group.alpha.LerpTo(1f, update: f =>
            {
                group.alpha = f;
            }, ct: inner);
            group.interactable = true;
            return null;
        }

        public async UniTask<object> Hide(object input)
        {
            cts = cts.Link(default, out var inner);
            await group.alpha.LerpTo(0f, update: f =>
            {
                group.alpha = f;
            }, ct: inner);
            group.interactable = false;
            return null;
        }

    }
}