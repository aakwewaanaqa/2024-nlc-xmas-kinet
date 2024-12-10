using System;
using UnityEngine;

namespace GameUsed.Core
{
    public static class Program
    {
        private static readonly Lazy<BodySourceManager> lazyBodySrc =
            new(() => new GameObject("body src").AddComponent<BodySourceManager>());

        public static BodySourceManager BodySrc => lazyBodySrc.Value;
    }
}