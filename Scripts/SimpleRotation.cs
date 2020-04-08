using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    public class SimpleRotation : MonoBehaviour
    {
        [SerializeField] Vector3 DegreesPerSecond;

        private void Update()
        {
            transform.localEulerAngles += DegreesPerSecond * Time.deltaTime;
        }
    }
}