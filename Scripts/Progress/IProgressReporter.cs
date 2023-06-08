using System;
using UnityEngine;

namespace JimmysUnityUtilities.Progress
{
    public interface IProgressReporter
    {
        void SetCompletionFraction(float completionFraction);
        void SetStatusMessage(string statusMessage);

        void FinishedSuccessfully();
        void FinishedWithError(Exception error = null);

        bool HasFinishedSuccessfully { get; }
        bool HasFinishedWithError { get; }
    }
}