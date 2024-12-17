using System.Linq;
using System.Threading;
using Windows.Kinect;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;

namespace GameUsed.Scenes.Bootstrap
{
    [AddComponentMenu("GameUsed/Scenes/Bootstrap/Hands View")]
    public class HandsView : MonoBehaviour
    {
        [SerializeField] private RectTransform rightHand;
        [SerializeField] private RectTransform leftHand;

        private static CancellationTokenSource cts { get; set; }
        
        public async UniTask<object> Show(object input)
        {
            Body activeBody = null;
            gameObject.SetActive(true);

            cts = cts.Link(default, out var inner);
            while (!inner.IsCancellationRequested)
            {
                await UniTask.Yield();
                var data = Program.BodySrc.GetData();
                if (data is null) continue;
                if (data.Length == 0) continue;
                activeBody ??= data.FirstOrDefault(b => b.IsTracked);
                activeBody.Let(body =>
                {
                    if (body is null) return;
                    if (!body.IsTracked) activeBody = null;
                    
                    var rightHandPos = body.Joints[JointType.HandRight].Position;
                    var leftHandPos = body.Joints[JointType.HandLeft].Position;
                    rightHand.anchoredPosition = new Vector2(rightHandPos.X * 1000, rightHandPos.Y * 1000);
                    leftHand.anchoredPosition = new Vector2(leftHandPos.X * 1000, leftHandPos.Y * 1000);
                });
            }
            
            return null;
        }
    }
}