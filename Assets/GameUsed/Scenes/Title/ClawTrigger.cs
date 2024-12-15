using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    public class ClawTrigger : MonoBehaviour
    {
        [SerializeField] private GameUsed.Core.Bounds bounds;

        private CancellationTokenSource cts  { get; set; } = new();
        private Gift                    gift { get; set; }

        public void FixToClaw()
        {
            bounds.OverlapBoxClosest(3, out var collided); // 碰撞檢測3個中最靠近的1個物體
            var parent = collided?.transform.parent;
            parent?.Let(it =>
            {
                gift = parent?.GetComponent<Gift>();
                if (gift is null) return;
                it.transform.SetParent(transform);
                it.transform.localPosition.LerpTo(bounds.Center, 5f, p =>
                {
                    it.transform.localPosition = p;    // 禮物移動到爪子中心
                }, cts.Token).Forget();                // 不要等
                Destroy(it.GetComponent<Rigidbody>()); // 銷毀禮物的剛體
                Destroy(collided);                     // 銷毀禮物的碰撞
                gift.PreventFromDestroy();             // 防止禮物被銷毀
            });
        }

        public Gift ReleaseClaw()
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
            return gift?.Apply(it =>
            {
                it.gameObject.AddComponent<Rigidbody>(); // 加上剛體，讓禮物掉落
                return gift;                             // 返回禮物
            });
        }
    }
}