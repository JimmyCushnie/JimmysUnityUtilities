using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class ClipboardAccess
    {
        static TextEditor textEditor = new TextEditor();

        public static void SetClipboardText(string text)
        {
            textEditor.text = text;
            textEditor.SelectAll();
            textEditor.Copy();
        }
    }
}