using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    public class ClawTrigger : MonoBehaviour
    {
        [SerializeField] private GameUsed.Core.Bounds bounds;

        private Gift gift;

        public void FixToClaw()
        {
            bounds.OverlapBoxClosest(3, out var collided);
            var parent = collided?.transform.parent;
            parent?.Let(it =>
            {
                gift = parent?.GetComponent<Gift>();
                if (gift is null) return;
                it.transform.SetParent(transform);
                it.transform.localPosition.LerpTo(bounds.Center, 5f, p => { it.transform.localPosition = p; }).Forget();
                Destroy(it.GetComponent<Rigidbody>());
                Destroy(collided);
            });
        }

        public Gift ReleaseClaw()
        {
            return gift?.Apply(it =>
            {
                it.transform.SetParent(null);
                it.gameObject.AddComponent<Rigidbody>();
                return gift;
            });
        }
    }
}