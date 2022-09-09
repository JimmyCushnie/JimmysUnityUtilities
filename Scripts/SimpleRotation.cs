using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    public class SimpleRotation : MonoBehaviour
    {
        public Vector3 DegreesPerSecond;
        public bool IgnoreTimescale;

        private void Update()
        {
            float frameMultiplier = IgnoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;

            transform.localEulerAngles += DegreesPerSecond * frameMultiplier;
        }
    }
}