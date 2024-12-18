using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameUsed.Core
{
    /// 為了讓在測試時可以用滑鼠互動，又能讓 Kinect 互動，製作的按鈕
    public class KButton : MonoBehaviour
    {
        [Tooltip("是否重複執行 OnClick")]
        [SerializeField] public bool isRepeatedly;

        [Tooltip("是不是可以互動")]
        [SerializeField] private bool isInteractable;
        
        [Tooltip("按下時執行的事件")]
        [SerializeField] public UnityEvent onClick;

        private RectTransform rectTransform { get; set; }
        private bool isInvoked { get; set; }
        
        public bool IsInteractable
        {
            get => isInteractable;
            set
            {
                isInvoked = false;
                isInteractable = value;
            }
        }

        public void OnCasted()
        {
            if (isInvoked) return;
            if (!isInteractable) return;
                
            onClick?.Invoke();
        }
        
        private void OnDrawGizmos()
        {
            rectTransform ??= GetComponent<RectTransform>();
            rectTransform.Let(it =>
            {
                var array = new Vector3[4];
                it.GetWorldCorners(array);
                Gizmos.color = Color.red;
                Gizmos.DrawLineStrip(array, true);
                Gizmos.color = Color.white;
            });
        }
    }
}