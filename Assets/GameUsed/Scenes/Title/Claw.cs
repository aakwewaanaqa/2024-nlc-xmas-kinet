using System;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("Title/Claw")]
    public class Claw : MonoBehaviour
    {
        private const float MAX_X = +0.9f;
        private const float MIN_X = -0.9f;

        [SerializeField] private float     speed;
        [SerializeField] private Transform clawL;
        [SerializeField] private Transform clawR;
        [SerializeField] private KButton   left;
        [SerializeField] private KButton   right;
        [SerializeField] private KButton   grab;

        private Rigidbody self { get; set; }

        private void OnEnable()
        {
            self = GetComponent<Rigidbody>();
            // 左右對於攝影機中的世界是顛倒的
            left.onClick.AddListener(() => Shift(+speed));
            right.onClick.AddListener(() => Shift(-speed));
        }

        private void Shift(float speed)
        {
            var position = self.position;
            position.x = Mathf.Clamp(position.x + speed * Time.deltaTime, MIN_X, MAX_X);
            self.MovePosition(position);
        }

        public async UniTask Grab()
        {
        }

        public async UniTask Release()
        {
        }
    }
}