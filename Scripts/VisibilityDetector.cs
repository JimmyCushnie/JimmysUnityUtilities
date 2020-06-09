using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    [RequireComponent(typeof(Renderer))]
    public class VisibilityDetector : MonoBehaviour
    {
        public bool IsVisible { get; private set; }

        public event Action OnBecomeVisible;
        public event Action OnBecomeInvisible;

        private void OnBecameVisible()
        {
            IsVisible = true;
            OnBecomeVisible?.Invoke();
        }

        private void OnBecameInvisible()
        {
            IsVisible = false;
            OnBecomeInvisible?.Invoke();
        }
    }
}

