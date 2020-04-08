using UnityEngine.UI;

namespace JimmysUnityUtilities
{
    public static class ScrollRectExtensions
    {
        public static void ScrollToBottom(this ScrollRect scrollRect)
            => scrollRect.verticalNormalizedPosition = 0;

        public static void ScrollToTop(this ScrollRect scrollRect)
            => scrollRect.verticalNormalizedPosition = 1;
    }
}