using System.Threading;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("Title/Title View")]
    public class TitleView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup group;
        [SerializeField] public  KButton     startGame;

        private CancellationTokenSource cts { get; set; }

        public async UniTask<object> Show(object input)
        {
            cts = cts.Link(default, out var inner);
            
            gameObject.SetActive(true);
            
            await group.alpha.LerpTo(1f, 3f, f =>
            {
                group.alpha = f;
            }, ct: inner);
            
            return null;
        }

        /// 用於等待開始遊戲
        public async UniTask WaitForTouch()
        {
            var isWaiting = true;
            
            startGame.onClick.RemoveAllListeners();
            startGame.onClick.AddListener(() =>
            {
                Debug.Log("start game touched.");
                isWaiting = false;
            });
            startGame.IsInteractable = true;
            
            await UniTask.WaitWhile(() => isWaiting);
        }

        public async UniTask<object> Hide(object input)
        {
            cts = cts.Link(default, out var inner);
            await group.alpha.LerpTo(0f, 3f, f =>
            {
                group.alpha = f;
            }, ct: inner);
            group.interactable = false;
            gameObject.SetActive(false);
            return null;
        }
    }
}