using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class ColliderExtensions
    {
        public static void SetEnabled<T>(this IEnumerable<T> colliders, bool value) where T : Collider
        {
            if (colliders == null)
                return;

            foreach (var collider in colliders)
                collider.enabled = value;
        }

        public static Vector3[] GetTopCornerPointsWorldspace(this BoxCollider box)
        {
            var points = new Vector3[4];

            points[0] = box.transform.TransformPoint(box.center + new Vector3(-box.size.x, box.size.y, -box.size.z) * 0.5f);
            points[1] = box.transform.TransformPoint(box.center + new Vector3(-box.size.x, box.size.y, box.size.z) * 0.5f);
            points[2] = box.transform.TransformPoint(box.center + new Vector3(box.size.x, box.size.y, -box.size.z) * 0.5f);
            points[3] = box.transform.TransformPoint(box.center + new Vector3(box.size.x, box.size.y, box.size.z) * 0.5f);

            return points;
        }
    }
}