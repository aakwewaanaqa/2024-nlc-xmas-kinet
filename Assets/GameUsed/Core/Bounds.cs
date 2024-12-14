using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameUsed.Core
{
    [AddComponentMenu("Core/Bounds")]
    public class Bounds : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Bounds bounds;

        public Vector3 GetRandomPosition()
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            float x = UnityEngine.Random.Range(min.x, max.x);
            float y = UnityEngine.Random.Range(min.y, max.y);
            float z = UnityEngine.Random.Range(min.z, max.z);

            return transform.TransformPoint(new Vector3(x, y, z));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var center = transform.TransformPoint(bounds.center);
            Gizmos.DrawWireCube(center, Vector3.Scale(bounds.size, transform.lossyScale));
        }
    }
}