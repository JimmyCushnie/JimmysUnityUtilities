using System;
using System.Collections.Generic;
using TMPro;

namespace JimmysUnityUtilities.EqualityComparers
{
    public class TMP_ColorGradientEqualityComparer : EqualityComparer<TMP_ColorGradient>
    {
        public static TMP_ColorGradientEqualityComparer Instance { get; } = new TMP_ColorGradientEqualityComparer();

        public override bool Equals(TMP_ColorGradient x, TMP_ColorGradient y)
        {
            if (Object.ReferenceEquals(x, y)) // Handles if both are null as well as if both are the same instance
                return true;

            if (x == null || y == null)
                return false;

            return x.colorMode == y.colorMode && x.topLeft == y.topLeft && x.topRight == y.topRight && x.bottomLeft == y.bottomLeft && x.bottomRight == y.bottomRight;
        }

        public override int GetHashCode(TMP_ColorGradient obj)
        {
            if (obj == null)
                return 4327;

            int hashCode = -941537779;
            hashCode = hashCode * -1521134295 + obj.colorMode.GetHashCode();
            hashCode = hashCode * -1521134295 + obj.topLeft.GetHashCode();
            hashCode = hashCode * -1521134295 + obj.topRight.GetHashCode();
            hashCode = hashCode * -1521134295 + obj.bottomLeft.GetHashCode();
            hashCode = hashCode * -1521134295 + obj.bottomRight.GetHashCode();
            return hashCode;
        }
    }
}
