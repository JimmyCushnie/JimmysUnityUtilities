using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class BehaviourExtensions
    {
        public static void SetEnabled<T>(this ICollection<T> behaviours, bool enabled) where T : Behaviour
        {
            foreach (var behaviour in behaviours.OrEmptyIfNull())
                behaviour.enabled = enabled;
        }
    }
}