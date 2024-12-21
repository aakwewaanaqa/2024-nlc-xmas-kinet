using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameUsed.Core
{
    [AddComponentMenu("GameUsed/Core/Canvas Set")]
    public class CanvasSet : MonoBehaviour
    {
        private enum ScaleMode
        {
            ConstantPixelSize,
            Expand,
        }
        
        [SerializeField] private float distance;
        [SerializeField] private int order;
        [SerializeField] private ScaleMode scaleMode;

        private void OnValidate()
        {
            GetComponent<Canvas>()?.Let(it =>
            {
                it.planeDistance = distance;
                it.sortingOrder = order;
            });
            
            GetComponent<CanvasScaler>()?.Let(it =>
            {
                it.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                it.uiScaleMode = scaleMode == ScaleMode.ConstantPixelSize
                    ? CanvasScaler.ScaleMode.ConstantPixelSize
                    : CanvasScaler.ScaleMode.ScaleWithScreenSize;
                it.referenceResolution = new Vector2(1920, 1080);
            });
        }
    }
}