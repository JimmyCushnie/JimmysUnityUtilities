using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class ColorExtensions
    {
        public static Color24 WithoutOpacity(this Color32 rgba)
            => new Color24(rgba.r, rgba.g, rgba.b);

        public static Color24 WithoutOpacity(this Color rgba)
        {
            var color32 = (Color32)rgba;
            return color32.WithoutOpacity();
        }



        public static Color32 WithAlpha(this Color32 rgba, byte newAlpha)
            => new Color32(rgba.r, rgba.g, rgba.b, newAlpha);

        public static Color WithAlpha(this Color rgba, float newAlpha)
            => new Color(rgba.r, rgba.g, rgba.b, newAlpha);
    }
}