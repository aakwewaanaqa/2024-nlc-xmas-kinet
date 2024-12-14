using System;
using UnityEngine;

namespace GameUsed.Core
{
    public static class Program
    {
        private static readonly Lazy<BodySourceManager> lazyBodySrc =
            new(() => new GameObject("body src")
                   .AddComponent<BodySourceManager>());

        private static readonly Lazy<WiperView> lazyWiper =
            new(() => Resources
                   .Load<GameObject>("wiper view")
                   .GetComponent<WiperView>());

        public static BodySourceManager BodySrc   => lazyBodySrc.Value;
        public static WiperView         WiperView => lazyWiper.Value;
    }
}