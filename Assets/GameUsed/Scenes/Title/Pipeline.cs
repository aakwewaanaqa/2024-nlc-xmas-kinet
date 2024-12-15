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
        internal static class Pipeline
        {
            public static TitleView    titleView;    // 標題畫面
            public static ClawView     clawView;     // 操控夾子的按鈕
            public static GiftProvider giftProvider; // 禮物提供產生器
            public static string       blessing;     // 夾到的祝福語

            /// <summary>
            ///     流程流水線入口
            /// </summary>
            public static Pipe Entry =>
                ShowTitle.Then(                   // 顯示標題畫面
                    WaitForTouch.Then(            // 開頭畫面等待觸碰標題
                        WaitForGiftReceived.Then( // 等待夾到禮物
                            ShowReceivedGift      // 顯示禮物
                        )
                    )
                );

            private static Pipe ShowReceivedGift => async () =>
            {
                Debug.Log("ShowReceiveGift");
                Debug.Log(blessing);
                return default;
            };

            private static Pipe ShowTitle => async () =>
            {
                Debug.Log("ShowTitle");
                giftProvider.Begin(null).Forget();
                await titleView.Show(null);
                return default;
            };

            private static Pipe WaitForTouch => async () =>
            {
                Debug.Log("WaitForTouch");
                await titleView.WaitForTouch();
                return default;
            };

            private static Pipe WaitForGiftReceived => async () =>
            {
                Debug.Log("WaitForGiftReceived");
                titleView.Hide(null).Forget();
                clawView.Show(null).Forget();
                await UniTask.WaitWhile(() => string.IsNullOrEmpty(blessing)); // 等待夾到禮物
                await giftProvider.Stop(null);
                return default;
            };
        }
    }
}