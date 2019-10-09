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
                    _Instance = new GameObject("coroutine runner").AddComponent<CoroutineUtility>();

                return _Instance;
            }
        }

        public static Coroutine Run(IEnumerator enumerator)
        {
            return Instance.StartCoroutine(enumerator);
        }
        public static void StopRunning(Coroutine coroutine)
        {
            if (coroutine == null) return;
            Instance.StopCoroutine(coroutine);
        }
    }
}