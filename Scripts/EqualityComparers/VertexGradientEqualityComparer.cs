using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JimmysUnityUtilities.EqualityComparers
{
    public class VertexGradientEqualityComparer : EqualityComparer<VertexGradient>
    {
        public static VertexGradientEqualityComparer Instance { get; } = new VertexGradientEqualityComparer();

        public override bool Equals(VertexGradient x, VertexGradient y)
            => x.topLeft == y.topLeft && x.topRight == y.topRight && x.bottomLeft == y.bottomLeft && x.bottomRight == y.bottomRight;

        public override int GetHashCode(VertexGradient obj)
        {
            int hashCode = -267303838;
            hashCode = hashCode * -1521134295 + obj.topLeft.GetHashCode();
            hashCode = hashCode * -1521134295 + obj.topRight.GetHashCode();
            hashCode = hashCode * -1521134295 + obj.bottomLeft.GetHashCode();
            hashCode = hashCode * -1521134295 + obj.bottomRight.GetHashCode();
            return hashCode;
        }
    }
}
