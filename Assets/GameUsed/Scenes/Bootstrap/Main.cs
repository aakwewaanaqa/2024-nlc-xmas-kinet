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
    public partial class Main : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI logger;

        private void Start()
        {
            Application.logMessageReceived += OnLogMessageReceived;
            
            UniTask.Create(async () =>
            {
                await Pipeline.Entry.Engage();
                await SceneTransitioner.Load("title");
            }).Forget();
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            logger.text = $"{condition}";
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

    }
}