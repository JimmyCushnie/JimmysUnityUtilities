using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class Vector2Extensions
    {
        ///<summary> rotates a point about the origin. </summary>
        public static Vector2 Rotate(this Vector2 point, float degreesCounterClockwise)
            => point.RotateAbout(Vector2.zero, degreesCounterClockwise);

        ///<summary> rotates a point about another point. </summary>
        public static Vector2 RotateAbout(this Vector2 point, Vector2 pivot, float degreesCounterClockwise)
        {
            float radians = degreesCounterClockwise * Mathf.Deg2Rad;

            float cosTheta = Mathf.Cos(radians);
            float sinTheta = Mathf.Sin(radians);
            float diffX = point.x - pivot.x;
            float diffY = point.y - pivot.y;

            return new Vector2
            (
                cosTheta * diffX - sinTheta * diffY + pivot.x,
                sinTheta * diffX + cosTheta * diffY + pivot.y
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


        public static Vector3 XYtoXZ(this Vector2 value) => new Vector3(value.x, 0, value.y);
    }
}