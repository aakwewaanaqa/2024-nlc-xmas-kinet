using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;
using GameUsed.Scenes.Bootstrap;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("GameUsed/Scenes/Title/Main")]
    public partial class Main : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] private HandsView hands;
        [SerializeField] private TitleView title;
        [SerializeField] private ClawView claw;
        [SerializeField] private GiftProvider giftProvider;
        [SerializeField] private ResultView result;
        
        [Header("UI")]
        [SerializeField] private Camera mainCamera;

        private void Start()
        {
            Pipeline.titleView = title;
            Pipeline.clawView = claw;
            Pipeline.hands = hands;
            Pipeline.giftProvider = giftProvider;
            Pipeline.resultView = result;

            UniTask.Create(DoPipeline).Forget();
            UniTask.Create(DoUI).Forget();
        }

        private async UniTask DoPipeline()
        {
            var ct = this.GetCancellationTokenOnDestroy();
            var r = await Pipeline.Entry();
            while (!r.IsEnd)
            {
                ct.ThrowIfCancellationRequested();

                await UniTask.Yield();
                
                r = await r.Continue();
            }
        }

        private async UniTask DoUI()
        {
            var ct = this.GetCancellationTokenOnDestroy();
            while (true)
            {
                ct.ThrowIfCancellationRequested();
                
                await UniTask.WaitForFixedUpdate();
                
                var hits = new RaycastHit[3];
                var pos = hands.rightHand.position;
                var screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, pos);
                var ray = mainCamera.ScreenPointToRay(screenPoint);
                Physics.RaycastNonAlloc(ray, hits);
                hits.Select(h => h.collider?.GetComponent<KButton>())
                    .Where(k => k.IsObject()).ToList()
                    .ForEach(k =>
                {
                    k.OnCasted();
                });
            }
        }
    }
}