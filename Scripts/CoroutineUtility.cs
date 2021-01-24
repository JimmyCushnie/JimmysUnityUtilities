using System;
using System.Collections;
using UnityEngine;

namespace JimmysUnityUtilities
{
    /// <summary>
    /// Run coroutines from static methods or from disabled gameobjects.
    /// </summary>
    public class CoroutineUtility : MonoBehaviour
    {
        private static CoroutineUtility _Instance = null;
        private static CoroutineUtility Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GameObject("coroutine runner").AddComponent<CoroutineUtility>();
                    DontDestroyOnLoad(_Instance);
                }

                return _Instance;
            }
        }

        public static Coroutine Run(IEnumerator enumerator)
        {
            Coroutine coroutine = null;
            Dispatcher.Invoke(() =>
            {
                coroutine = Instance.StartCoroutine(enumerator);
            });

            return coroutine;
        }
        public static void StopRunning(Coroutine coroutine)
        {
            if (coroutine == null) 
                return;

            Dispatcher.Invoke(() =>
            {
                Instance.StopCoroutine(coroutine);
            });
        }



        public static void RunAfterOneFrame(Action action)
            => RunAfterFrameDelay(action, 1);

        public static void RunAfterFrameDelay(Action action, int framesToWait)
        {
            Run(routine());
            IEnumerator routine()
            {
                for (int i = 0; i < framesToWait; i++)
                    yield return new WaitForEndOfFrame();

                action.Invoke();
            }
        }

        public static void RunAfterSecondsDelay(Action action, float secondsToWait)
        {
            Run(routine());
            IEnumerator routine()
            {
                yield return new WaitForSeconds(secondsToWait);

                action.Invoke();
            }
        }

        public static void RunAfterDelay(Action action, TimeSpan delay)
            => RunAfterSecondsDelay(action, (float)delay.TotalSeconds);
    }
}