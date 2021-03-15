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
    }
}