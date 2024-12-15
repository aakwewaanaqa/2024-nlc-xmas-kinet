using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameUsed.Core
{
    [AddComponentMenu("Core/Bounds")]
    public class Bounds : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Bounds bounds;

        public Vector3 Center => bounds.center;

        public Vector3 GetRandomPosition()
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            float x = UnityEngine.Random.Range(min.x, max.x);
            float y = UnityEngine.Random.Range(min.y, max.y);
            float z = UnityEngine.Random.Range(min.z, max.z);

            return transform.TransformPoint(new Vector3(x, y, z));
        }

        public void OverlapBoxClosest(int count, out Collider closest)
        {
            var hits = new Collider[count];
            var center  = transform.position + bounds.center;
            var extents = bounds.extents / 2f;
            Physics.OverlapBoxNonAlloc(center, extents, hits);
            closest = hits
               .ToList()
               .Where(h => h is object && !h.Equals(null))
               .OrderBy(h => (h.transform.position - center).sqrMagnitude)
               .FirstOrDefault();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color  = Color.red;
            var center  = transform.position + bounds.center;
            var extents = bounds.extents;
            Gizmos.DrawWireCube(center, extents);
        }
    }
}