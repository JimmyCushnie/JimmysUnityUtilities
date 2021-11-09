using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    // Because Unity are negligent fools, they did not include a built-in == operator or .Equals() method for Color32.
    // However, SOMETIMES YOU NEED TO FREAKING COMPARE TWO COLOR32s.

    public class Color32EqualityComparer : IEqualityComparer<Color32>
    {
        public static Color32EqualityComparer Instance { get; } = new Color32EqualityComparer();

        public bool Equals(Color32 x, Color32 y)
            => x.r == y.r && x.g == y.g && x.b == y.b && x.a == y.a;

        public int GetHashCode(Color32 obj)
            => (((13 * 7 + obj.r) * 7 + obj.g) * 7 + obj.b) * 7 + obj.a;
    }
}
