using System;
using System.Globalization;
using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class Color24Extensions
    {
        public static void ToHSV(this Color24 color, out float hue, out float saturation, out float value)
            => Color.RGBToHSV(color.WithOpacity(), out hue, out saturation, out value);
    }
}
