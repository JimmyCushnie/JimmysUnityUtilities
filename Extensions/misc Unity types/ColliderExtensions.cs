using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class ColliderExtensions
    {
        public static void SetEnabled(this ICollection<Collider> colliders, bool value)
        {
            foreach (var collider in colliders)
                collider.enabled = value;
        }
    }
}