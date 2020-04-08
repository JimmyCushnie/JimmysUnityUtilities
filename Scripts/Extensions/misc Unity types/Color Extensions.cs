using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class Color32Extensions
    {
        public static Color24 WithoutOpacity(this Color32 rgba)
            => new Color24(rgba.r, rgba.g, rgba.b);
    }

    public static class ColorExtensions
    {
        public static Color24 WithoutOpacity(this Color rgba)
        {
            var color32 = (Color32)rgba;
            return color32.WithoutOpacity();
        }
    }
}