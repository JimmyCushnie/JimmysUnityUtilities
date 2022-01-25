using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class ClipboardAccess
    {
        public static void SetClipboardText(string text)
        {
            GUIUtility.systemCopyBuffer = text;
        }
    }
}