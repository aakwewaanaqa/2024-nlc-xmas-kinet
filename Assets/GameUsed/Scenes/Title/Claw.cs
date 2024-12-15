using System;
using System.Linq;
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
        [SerializeField] private ClawTrigger trigger;
        [SerializeField] private Vector3     originalPlace;

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
            await OpenClaw();                 // 張開爪子
            await ProlongSpring();            // 伸展彈簧
            trigger.FixToClaw();              // 把禮物固定在爪子上
            await CloseClaw();                // 夾起禮物
            await ShortenSpring();            // 收縮彈簧
            await BackToPlace();              // 回到原位
            await OpenClaw();                 // 張開爪子
            var gift = trigger.ReleaseClaw(); // 釋放禮物
            await UniTask.Delay(1000);        // 等待1.0秒
            return gift;
        }

        private async UniTask<object> OpenClaw()
        {
            animator.SetBool(open, true); // 張開爪子
            return null;
        }

        private async UniTask<object> CloseClaw()
        {
            animator.SetBool(open, false); // 收起開爪子
            await UniTask.Delay(1000);     // 等待1.0秒
            return null;
        }

        private async UniTask<object> ProlongSpring()
        {
            await spring.maxDistance.LerpTo(1.0f, 3f, f => { spring.maxDistance = f; }); // 伸展彈簧
            await UniTask.Delay(1500);                                                   // 等待1.5秒
            return null;
        }

        private async UniTask<object> ShortenSpring()
        {
            await spring.maxDistance.LerpTo(0f, 3f, f => { spring.maxDistance = f; }); // 收縮彈簧
            await UniTask.Delay(1000);                                                 // 等待1.2秒
            return null;
        }

        private async UniTask<object> BackToPlace()
        {
            await transform.position.MoveTo(originalPlace, 1f, p => { transform.position = p; });
            return null;
        }

        private async UniTask<object> ReleaseGift(Gift gift)
        {
            return null;
        }
    }
}