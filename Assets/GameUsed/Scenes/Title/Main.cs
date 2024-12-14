using System;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("GameUsed/Scenes/Title/Main")]
    public partial class Main : MonoBehaviour
    {
        [SerializeField] public TitleView    title;
        [SerializeField] public ClawView     claw;
        [SerializeField] public GiftProvider giftProvider;
        [SerializeField] public GiftReceiver giftReceiver;

        private async UniTask Start()
        {
            Pipeline.title        = title;
            Pipeline.claw         = claw;
            Pipeline.giftProvider = giftProvider;
            Pipeline.giftReceiver = giftReceiver;
            
            var r           = await Pipeline.Entry();
            if (!r.IsEnd) r = await r.Continue();
            if (r.Ex is ToTitle) await SceneTransitioner.Load("title");
        }
    }
}