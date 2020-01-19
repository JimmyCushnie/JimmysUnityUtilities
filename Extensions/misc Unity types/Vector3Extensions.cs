using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class Vector3Extensions
    {
        public static Vector3 ClampDimensions(this Vector3 value, Vector3 min, Vector3 max)
        {
            return new Vector3
                (
                Mathf.Clamp(value.x, min.x, max.x),
                Mathf.Clamp(value.y, min.y, max.y),
                Mathf.Clamp(value.z, min.z, max.z)
                );
        }

        public static Vector3 CapRange(this Vector3 value, float maxX, float maxY, float maxZ)
            => CapRange(value, new Vector3(maxX, maxY, maxZ));

        public static Vector3 CapRange(this Vector3 value, Vector3 max)
        {
            return new Vector3
                (
                value.x.CapRange(max.x),
                value.y.CapRange(max.y),
                value.z.CapRange(max.z)
                );
        }
    }
}