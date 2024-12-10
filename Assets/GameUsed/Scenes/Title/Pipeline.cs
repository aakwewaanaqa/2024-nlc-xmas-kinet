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
            public static TitleView    title;
            public static ClawView     claw;
            public static GiftProvider giftProvider;
            public static GiftReceiver giftReceiver;

            /// <summary>
            ///     流程流水線入口
            /// </summary>
            public static PipeFunc Entry =>
                AsTitle.Then(
                    WaitForTouch.RetryThen(
                        WaitForGiftReceived.Then(
                            AsReceiveGift,
                            async () => PipeReturn.Except(new ToTitle())
                            )
                        )
                    );

            private static PipeFunc AsReceiveGift => async () =>
            {
                return default;
            };

            private static PipeFunc AsTitle => async () =>
            {
                await giftProvider.Begin(null);
                await title.Show(null);
                return default;
            };

            private static PipeFunc WaitForTouch => async () =>
            {
                return default;
            };

            private static PipeFunc WaitForGiftReceived => async () =>
            {
                await giftReceiver.Begin(null);
                await giftProvider.Stop(null);
                return default;
            };
        }
    }
}