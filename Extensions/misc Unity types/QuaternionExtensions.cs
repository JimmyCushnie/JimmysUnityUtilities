using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class QuaternionExtensions
    {
        public static Quaternion Inverse(this Quaternion value)
            => Quaternion.Inverse(value);

        public static Quaternion ClampRotationAroundXAxis(this Quaternion q, float angleMin = -90f, float angleMax = +90f)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, angleMin, angleMax);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}