using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class RectTransformExtensions
    {
        public static void SetMarginLeft(this RectTransform rt, float left)
            => rt.offsetMin = new Vector2(left, rt.offsetMin.y);

        public static void SetMarginRight(this RectTransform rt, float right)
            => rt.offsetMax = new Vector2(-right, rt.offsetMax.y);

        public static void SetMarginTop(this RectTransform rt, float top)
            => rt.offsetMax = new Vector2(rt.offsetMax.x, -top);

        public static void SetMarginBottom(this RectTransform rt, float bottom)
            => rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}