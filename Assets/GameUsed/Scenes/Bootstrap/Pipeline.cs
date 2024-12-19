using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;

namespace GameUsed.Scenes.Bootstrap
{
    public partial class Main
    {
        internal static class Pipeline
        {
            public static Pipe Entry =>
                PrintAPI.Then(
                    PrintDaemon
                );

            /// 確認有抓到祝福
            private static Pipe PrintAPI => async () =>
            {
                var blessings = await Program.Blessing;
                var flag      = blessings.Count > 0;
                if (!flag) return PipeReturn.Except(new Exception("沒有抓到祝福。。"));

                Debug.Log($"已抓到祝福：{blessings.Count}筆");
                await UniTask.Delay(1000);

#if UNITY_EDITOR
                var charList = new List<char>();
                // 額外的標點符號
                "！？。，；".ToList().ForEach(c =>
                {
                    charList.Add(c);
                });
                // 會用到的字元
                foreach (var blessing in blessings)
                {
                    charList.AddRange(blessing.Where(c => !charList.Contains(c)));
                    Debug.Log(blessing);
                    await UniTask.Yield();
                };

                var             file   = Path.Combine(Application.dataPath, "used characters.txt");
                await using var writer = new StreamWriter(File.Create(file));
                charList.ForEach(c => writer.Write(c));
#endif
                return default;
            };
            
            /// 確認有預備好要連接 Kinect 但是還是要開 Kinect Studio
            private static Pipe PrintDaemon => async () => 
            {
                var bodySrc = Program.BodySrc;
                await UniTask.WaitUntil(() => bodySrc.IsReady);
                
                Debug.Log("Kinect daemon 預備好了！");
                await UniTask.Delay(1000);
                
                return default;
            };
        }
    }
}