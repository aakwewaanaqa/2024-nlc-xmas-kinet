using System;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("GameUsed/Scenes/Title/Main")]
    public partial class Main : MonoBehaviour
    {
        [SerializeField] private TitleView    title;
        [SerializeField] private GiftProvider giftDrop;

        private async UniTask Start()
        {
            var r           = await Pipeline.Entry();
            if (!r.IsEnd) r = await r.Continue();
            if (r.Ex is ToTitle) await SceneTransitioner.Load("title");
        }
    }

    public class GiftProvider : MonoBehaviour
    {
        public async UniTask Begin(object input)
        {
            await UniTask.Yield();
        }

        public async UniTask Stop(object input)
        {
            await UniTask.Yield();
        }
    }

    public class TitleView : MonoBehaviour
    {
        public async UniTask<object> Show(object input)
        {
            await UniTask.Yield();
            return null;
        }

        public async UniTask<object> Hide(object input)
        {
            await UniTask.Yield();
            return null;
        }
    }
}