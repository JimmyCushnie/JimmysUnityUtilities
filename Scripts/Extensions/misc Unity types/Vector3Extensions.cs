using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class Vector3Extensions
    {
        public static Vector3 ClampDimensions(this Vector3 value, Vector3 minInclusive, Vector3 maxInclusive)
        {
            return new Vector3
                (
                value.x.Clamp(minInclusive.x, maxInclusive.x),
                value.y.Clamp(minInclusive.y, maxInclusive.y),
                value.z.Clamp(minInclusive.z, maxInclusive.z)
                );
        }

        public static bool IsBetween(this Vector3 value, Vector3 minInclusive, Vector3 maxInclusive)
        {
            return
                value.x.IsBetween(minInclusive.x, maxInclusive.x) &&
                value.y.IsBetween(minInclusive.y, maxInclusive.y) &&
                value.z.IsBetween(minInclusive.z, maxInclusive.z);
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


        public static bool IsPrettyCloseTo(this Vector3 vectorA, Vector3 vectorB, float maxDistance = 0.01f)
            => Vector3.Distance(vectorA, vectorB) <= maxDistance;

        public static bool IsPrettyCloseToPointingInTheSameDirectionAs(this Vector3 vectorA, Vector3 vectorB, float dotMarginOfError = 0.05f)
            => Vector3.Dot(vectorA, vectorB).IsPrettyCloseTo(1, dotMarginOfError);

        public static bool IsPrettyCloseToPointingInTheOppositeDirectionAs(this Vector3 vectorA, Vector3 vectorB, float dotMarginOfError = 0.05f)
            => Vector3.Dot(vectorA, vectorB).IsPrettyCloseTo(-1, dotMarginOfError);

        public static bool IsPrettyCloseToBeingPerpendicularWith(this Vector3 vectorA, Vector3 vectorB, float dotMarginOfError = 0.05f)
            => Vector3.Dot(vectorA, vectorB).IsPrettyCloseTo(0, dotMarginOfError);

        public static bool IsPrettyCloseToPointingAlongSameAxisAs(this Vector3 vectorA, Vector3 vectorB, float dotMarginOfError = 0.05f)
            => vectorA.IsPrettyCloseToPointingInTheSameDirectionAs(vectorB, dotMarginOfError) || vectorA.IsPrettyCloseToPointingInTheOppositeDirectionAs(vectorB, dotMarginOfError);


        public static Vector2 RemoveZComponent(this Vector3 vector3)
            => new Vector2(vector3.x, vector3.y);


        /// <summary>
        /// Returns the vector closest to the input which is 1 or -1 on one axis, and 0 on all the other axes.
        /// </summary>
        public static Vector3 RoundToNearestCardinalValue(this Vector3 vector3)
        {
            float absX = Mathf.Abs(vector3.x);
            float absY = Mathf.Abs(vector3.y);
            float absZ = Mathf.Abs(vector3.z);

            // This code is pretty bad lmao
            if (absX > absY)
            {
                if (absX > absZ)
                {
                    float signX = Mathf.Sign(vector3.x);
                    return new Vector3(signX, 0, 0);
                }
                else
                {
                    float signZ = Mathf.Sign(vector3.z);
                    return new Vector3(0, 0, signZ);
                }
            }
            else
            {
                if (absY > absZ)
                {
                    float signY = Mathf.Sign(vector3.y);
                    return new Vector3(0, signY ,0);
                }
                else
                {
                    float signZ = Mathf.Sign(vector3.z);
                    return new Vector3(0, 0, signZ);
                }
            }
        }


        public static Vector3 ScaleBy(this Vector3 vector3, Vector3 scaler) => Vector3.Scale(vector3, scaler);


        public static Vector2Int RoundToInts(this Vector2 vector)
            => new Vector2Int(vector.x.RoundToInt(), vector.y.RoundToInt());

        public static Vector3Int RoundToInts(this Vector3 vector)
            => new Vector3Int(vector.x.RoundToInt(), vector.y.RoundToInt(), vector.z.RoundToInt());


        public static Vector3 ScaleBy(this Vector3Int vector, float factor)
            => new Vector3(vector.x * factor, vector.y * factor, vector.z * factor);
    }
}