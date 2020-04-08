using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class Vector2Extensions
    {
        ///<summary> rotates a point about the origin. </summary>
        public static Vector2 Rotate(this Vector2 point, float degrees)
            => point.RotateAbout(Vector2.zero, degrees);

        ///<summary> rotates a point about another point. </summary>
        public static Vector2 RotateAbout(this Vector2 point, Vector2 pivot, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float CosTheta = Mathf.Cos(radians);
            float SinTheta = Mathf.Sin(radians);
            float DiffX = point.x - pivot.x;
            float DiffY = point.y - pivot.y;

            return new Vector2
            (
                CosTheta * DiffX - SinTheta * DiffY + pivot.x,
                SinTheta * DiffX + CosTheta * DiffY + pivot.y
            );
        }

        public static Vector2 ClampDimensions(this Vector2 value, Vector2 min, Vector2 max)
        {
            return new Vector2
                (
                Mathf.Clamp(value.x, min.x, max.x),
                Mathf.Clamp(value.y, min.y, max.y)
                );
        }
    }
}