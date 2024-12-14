using System;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("Title/Claw")]
    public class Claw : MonoBehaviour
    {
        private static readonly int   open  = Animator.StringToHash("open");
        private const           float MAX_X = +0.9f;
        private const           float MIN_X = -0.9f;

        [SerializeField] private float       speed;
        [SerializeField] private Transform   clawL;
        [SerializeField] private Transform   clawR;
        [SerializeField] private Rigidbody   root;   // 爪子懸掛的根基
        [SerializeField] private SpringJoint spring; // 爪子懸掛著彈簧，鬆開彈簧夾禮物
        [SerializeField] private Animator    animator;

        /// 爪子可以移動的方向
        public enum Direction
        {
            Left,
            Right
        }

        public void Shift(Direction direction)
        {
            var position = root.position;
            var polarity = direction == Direction.Left ? 1f : -1f;
            position.x = Mathf.Clamp(position.x + speed * polarity * Time.deltaTime, MIN_X, MAX_X);
            root.MovePosition(position);
        }

        public async UniTask<object> Grab()
        {
            animator.SetBool(open, true);
            await spring.maxDistance.LerpTo(1.2f, 3f, f =>
            {
                spring.maxDistance = f;
            });                            // 伸展彈簧
            animator.SetBool(open, false); // 夾起禮物
            await UniTask.Delay(1000);     // 等待一秒
            await spring.maxDistance.LerpTo(0f, 3f, f =>
            {
                spring.maxDistance = f;
            }); // 收縮彈簧
            // TODO: 還不知道怎麼抓到禮物
            return null;
        }

        public async UniTask Release()
        {
        }
    }
}