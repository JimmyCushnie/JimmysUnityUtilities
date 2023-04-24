using UnityEngine;
using System;

namespace JimmysUnityUtilities
{
    public static class PhysicsUtilities
    {
        /// <summary>
        /// Gets a mask of layers that collide with the given layer.
        /// </summary>
        public static LayerMask GetCollisionMaskOf(int layer)
        {
            if (layer >= 32)
                throw new ArgumentOutOfRangeException($"Supplied layer was out of range. Make sure you are passing a layer and not a layer mask.");

            var collisionMask = new LayerMask();
            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(layer, i))
                    collisionMask = collisionMask | 1 << i;
            }

            return collisionMask;
        }


        /// <summary>
        /// Returns <see langword="true"/> if <paramref name="pointWorldspace"/>, is inside <paramref name="collider"/>; else returns <see langword="false"/>.
        /// </summary>
        public static bool PointIsInsideCollider(Collider collider, Vector3 pointWorldspace)
        {
            var closestPoint = collider.ClosestPoint(pointWorldspace);

            const float epsilon = 0.0001f;
            return
                closestPoint.x.IsPrettyCloseTo(pointWorldspace.x, epsilon) &&
                closestPoint.y.IsPrettyCloseTo(pointWorldspace.y, epsilon) &&
                closestPoint.z.IsPrettyCloseTo(pointWorldspace.z, epsilon);
        }
    }
}