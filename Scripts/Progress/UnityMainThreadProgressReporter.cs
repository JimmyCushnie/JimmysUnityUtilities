using System;

namespace JimmysUnityUtilities.Progress
{
    public class UnityMainThreadProgressReporter : IProgressReporter
    {
        public bool HasFinishedSuccessfully { get; private set; } = false;
        public void FinishedSuccessfully()
        {
            HasFinishedSuccessfully = true;
            Dispatcher.InvokeAsync(() => OnFinishedSuccessfully?.Invoke());
        }
        private readonly Action OnFinishedSuccessfully;

        public bool HasFinishedWithError { get; private set; } = false;
        public void FinishedWithError(Exception error)
        {
            HasFinishedWithError = true;
            Dispatcher.InvokeAsync(() => OnFinishedWithError?.Invoke(error));
        }
        private readonly Action<Exception> OnFinishedWithError;

        public void SetCompletionFraction(float completionFraction) => Dispatcher.InvokeAsync(() => OnSetCompletionFraction?.Invoke(completionFraction));
        private readonly Action<float> OnSetCompletionFraction;

        public void SetStatusMessage(string statusMessage) => Dispatcher.InvokeAsync(() => OnSetStatusMessage?.Invoke(statusMessage));
        private readonly Action<string> OnSetStatusMessage;


        public UnityMainThreadProgressReporter(Action onFinishedSuccessfully, Action<Exception> onFinishedWithError, Action<float> onSetCompletionFraction, Action<string> onSetStatusMessage)
        {
            OnFinishedSuccessfully = onFinishedSuccessfully;
            OnFinishedWithError = onFinishedWithError;
            OnSetCompletionFraction = onSetCompletionFraction;
            OnSetStatusMessage = onSetStatusMessage;
        }
    }
}