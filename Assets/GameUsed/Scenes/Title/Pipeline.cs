using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    public partial class Main
    {
        /// <summary>
        ///     流程流水線
        /// </summary>
        private static class Pipeline
        {
            public static TitleView    title;        // 標題畫面
            public static ClawView     claw;         // 操控夾子的按鈕
            public static GiftProvider giftProvider; // 禮物提供產生器
            public static string       blessing;     // 夾到的祝福語

            /// <summary>
            ///     流程流水線入口
            /// </summary>
            public static PipeFunc Entry =>
                ShowTitle.Then(
                    WaitForTouch.Then(            // 開頭畫面等待觸碰標題
                        WaitForGiftReceived.Then( // 等待夾到禮物
                            ShowReceiveGift,
                            async () => PipeReturn.Except(new ToTitle())
                        )
                    )
                );

            private static PipeFunc ShowReceiveGift => async () =>
            {
                Debug.Log("ShowReceiveGift");
                return default;
            };

            private static PipeFunc ShowTitle => async () =>
            {
                Debug.Log("ShowTitle");
                giftProvider.Begin(null).Forget();
                await title.Show(null);
                return default;
            };

            private static PipeFunc WaitForTouch => async () =>
            {
                Debug.Log("WaitForTouch");
                await title.WaitForTouch();
                return default;
            };

            private static PipeFunc WaitForGiftReceived => async () =>
            {
                Debug.Log("WaitForGiftReceived");
                title.Hide(null).Forget();
                claw.Show(null).Forget();
                await UniTask.WaitWhile(() => string.IsNullOrEmpty(blessing)); // 等待夾到禮物
                await giftProvider.Stop(null);
                return default;
            };
        }
    }
}