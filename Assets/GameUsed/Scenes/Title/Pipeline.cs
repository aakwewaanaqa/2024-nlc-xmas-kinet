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
            public static GiftDropView giftDrop;

            /// <summary>
            ///     流水線入口
            /// </summary>
            public static PipeFunc Entry =>
                WaitForTouch;

            private static PipeFunc WaitForTouch => async () =>
            {
                await giftDrop.Begin(null);
                await title.Show(null);
                return default;
            };
        }
    }
}