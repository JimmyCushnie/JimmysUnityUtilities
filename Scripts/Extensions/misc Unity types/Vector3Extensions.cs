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

        public static bool IsPrettyCloseToPointingInTheSameDirectionAs(this Vector3 vectorA, Vector3 vectorB, float dotMarginOfError = 0.05f)
            => Vector3.Dot(vectorA, vectorB).IsPrettyCloseTo(1, dotMarginOfError);

        public static bool IsPrettyCloseToPointingInTheOppositeDirectionAs(this Vector3 vectorA, Vector3 vectorB, float dotMarginOfError = 0.05f)
            => Vector3.Dot(vectorA, vectorB).IsPrettyCloseTo(-1, dotMarginOfError);

        public static bool IsPrettyCloseToBeingPerpendicularWith(this Vector3 vectorA, Vector3 vectorB, float dotMarginOfError = 0.05f)
            => Vector3.Dot(vectorA, vectorB).IsPrettyCloseTo(0, dotMarginOfError);

        public static bool IsPrettyCloseToPointingAlongSameAxisAs(this Vector3 vectorA, Vector3 vectorB, float dotMarginOfError = 0.05f)
            => vectorA.IsPrettyCloseToPointingInTheSameDirectionAs(vectorB, dotMarginOfError) || vectorA.IsPrettyCloseToPointingInTheOppositeDirectionAs(vectorB, dotMarginOfError);
    }
}