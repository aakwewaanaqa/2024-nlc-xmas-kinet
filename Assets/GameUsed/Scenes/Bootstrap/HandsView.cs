using System;
using System.Linq;
using System.Threading;
using Windows.Kinect;
using Cysharp.Threading.Tasks;
using GameUsed.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GameUsed.Scenes.Bootstrap
{
    [AddComponentMenu("GameUsed/Scenes/Bootstrap/Hands View")]
    public class HandsView : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] public RectTransform rightHand;
        [SerializeField] private RectTransform leftHand;

        private static CancellationTokenSource cts { get; set; }
        
        public async UniTask<object> Show(object input)
        {
            Body activeBody = null;
            
            gameObject.SetActive(true);

            cts = cts.Link(default, out var inner);
            while (true)
            {
                inner.ThrowIfCancellationRequested();
                
                await UniTask.Yield();
                
                var data = Program.BodySrc.GetData();
                if (data is null || data.Length == 0)
                {
                    rightHand.gameObject.SetActive(true);
                    leftHand.gameObject.SetActive(false);
                    new Vector2(Input.mousePosition.x, Input.mousePosition.y).Let(pos =>
                    {
                        var parent = rightHand.parent as RectTransform;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, pos, mainCamera, out pos);
                        rightHand.anchoredPosition = new Vector2(pos.x, pos.y);
                    });
                    continue;
                }
                
                activeBody ??= data.FirstOrDefault(b => b.IsTracked);
                activeBody.Let(body =>
                {
                    if (body is null) return;
                    if (!body.IsTracked) activeBody = null;
                    
                    var rightHandPos = body.Joints[JointType.HandRight].Position;
                    var leftHandPos = body.Joints[JointType.HandLeft].Position;
                    rightHand.gameObject.SetActive(true);
                    leftHand.gameObject.SetActive(true);
                    rightHand.anchoredPosition = new Vector2(rightHandPos.X * 1000, rightHandPos.Y * 1000);
                    leftHand.anchoredPosition = new Vector2(leftHandPos.X * 1000, leftHandPos.Y * 1000);
                });
            }
            
            return null;
        }

        public async UniTask<object> Hide(object input)
        {
            await UniTask.Yield();
            return null;
        }
    }
}