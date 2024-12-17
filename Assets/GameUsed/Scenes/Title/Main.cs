using System;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;
using GameUsed.Scenes.Bootstrap;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("GameUsed/Scenes/Title/Main")]
    public partial class Main : MonoBehaviour
    {
        [SerializeField] public TitleView    title;
        [SerializeField] public ClawView     claw;
        [SerializeField] private HandsView   hands;
        [SerializeField] public GiftProvider giftProvider;
        [SerializeField] public GiftReceiver giftReceiver;

        private async UniTask Start()
        {
            Pipeline.titleView        = title;
            Pipeline.clawView         = claw;
            Pipeline.hands            = hands;
            Pipeline.giftProvider = giftProvider;

            var r              = await Pipeline.Entry();
            while (!r.IsEnd) r = await r.Continue();
            if (r.Ex is ToTitle) await SceneTransitioner.Load("title");
        }
    }
}