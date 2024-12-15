using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using TMPro;
using UnityEngine;

namespace GameUsed.Scenes.Bootstrap
{
    [AddComponentMenu("GameUsed/Scenes/Bootstrap/Main")]
    public class Main : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI logger;

        private async UniTask Start()
        {
            Application.logMessageReceived += OnLogMessageReceived;
            var r = await Pipeline.Entry.Engage();
            if (r.IsFaulty) Debug.LogError(r.Ex);
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            logger.text = $"{condition}";
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        internal static class Pipeline
        {
            public static Pipe Entry =>
                PrintAPI;

            private static Pipe PrintAPI => async () =>
            {
                var blessings = await Program.Blessing;
                var flag      = blessings.Count > 0;
                if (!flag) return PipeReturn.Except(new Exception("沒有抓到祝福。。"));

                Debug.Log($"已抓到祝福：{blessings.Count}筆");
                await UniTask.Delay(200);

                var charList = new List<char>();
                foreach (var blessing in blessings)
                {
                    charList.AddRange(blessing.Where(c => !charList.Contains(c)));
                    Debug.Log(blessing);
                    await UniTask.Delay(200);
                }

                var             file   = Path.Combine(Application.dataPath, "used characters.txt");
                await using var writer = new StreamWriter(File.Create(file));
                charList.ForEach(c => writer.Write(c));

                return default;
            };
        }
    }
}