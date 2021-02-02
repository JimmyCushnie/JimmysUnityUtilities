using System;
using TMPro;

namespace JimmysUnityUtilities
{
    public static class TMP_InputFieldExtensions
    {
        /// <summary>
        /// TextMeshPro does this brilliant thing where it triggers the <see cref="TMP_InputField.onSubmit"/> event both when you press
        /// enter to submit, and also when you press escape to cancel. If you subscribe to the event using this extension method instead,
        /// your callback will only be fired when the input field is ACTUALLY submitted (i.e. the user presses enter and not escape).
        /// </summary>
        public static void OnActuallySubmitted(this TMP_InputField inputField, Action<string> onActuallySubmittedCallback)
        {
            inputField.onSubmit.AddListener(text =>
            {
                if (!inputField.wasCanceled)
                    onActuallySubmittedCallback?.Invoke(text);
            });
        }
    }
}