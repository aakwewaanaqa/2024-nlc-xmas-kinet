using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace GameUsed.Core
{
    public static class Program
    {
        private const string BLESSING_API =
            "https://script.google.com/macros/s/AKfycbxSWP6KmoLJAY2yRMTBDWWlRpxpY6gUs6cGth0NQ3hNj40wj5RCXNGfJFtkTnF0PDca9Q/exec";

        private static readonly Lazy<BodySourceManager> lazyBodySrc =
            new(() => new GameObject("body src")
                   .AddComponent<BodySourceManager>());

        private static readonly Lazy<WiperView> lazyWiper =
            new(() => Resources
                   .Load<GameObject>("wiper view")
                   .GetComponent<WiperView>());

        private static UniLazy<IList<string>> blessings { get; } = new(GetBlessings);


        public static UniTask<IList<string>> Blessing  => blessings.Value;
        public static BodySourceManager      BodySrc   => lazyBodySrc.Value;
        public static WiperView              WiperView => lazyWiper.Value;

        private static async UniTask<IList<string>> GetBlessings()
        {
            var req  = new UnityWebRequest(BLESSING_API);
            req.downloadHandler = new DownloadHandlerBuffer();
            await req.SendWebRequest();
            var body   = req.downloadHandler.text;
            var splits = new Regex("-{2,100}", RegexOptions.Multiline).Split(body);
            return splits.Select(split => new Regex("(-|—).+").Replace(split, "").Trim()).ToList();
        }
    }
}