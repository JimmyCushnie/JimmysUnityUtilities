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


        public static void SetAnchoredPositionX(this RectTransform rt, float xPos)
            => rt.anchoredPosition = new Vector2(xPos, rt.anchoredPosition.y);

        public static void SetAnchoredPositionY(this RectTransform rt, float yPos)
            => rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, yPos);


        public static void SetAnchorMinX(this RectTransform rt, float value)
            => rt.anchorMin = new Vector2(value, rt.anchorMin.y);

        public static void SetAnchorMinY(this RectTransform rt, float value)
            => rt.anchorMin = new Vector2(rt.anchorMin.x, value);

        public static void SetAnchorMaxX(this RectTransform rt, float value)
            => rt.anchorMax = new Vector2(value, rt.anchorMax.y);

        public static void SetAnchorMaxY(this RectTransform rt, float value)
            => rt.anchorMax = new Vector2(rt.anchorMax.x, value);


        public static void SetPivotX(this RectTransform rt, float value)
            => rt.pivot = new Vector2(value, rt.pivot.y);

        public static void SetPivotY(this RectTransform rt, float value)
            => rt.pivot = new Vector2(rt.pivot.x, value);
    }
}