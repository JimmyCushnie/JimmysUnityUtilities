using System;
using System.Collections.Generic;
using Lidgren.Network;
using UnityEngine;
using System.ComponentModel;
using System.Linq.Expressions;

using UnityEngine.UI;
using Object = UnityEngine.Object;
using Component = UnityEngine.Component;
using Random = UnityEngine.Random;

namespace JimmysUnityUtilities
{
    public static class BehaviourExtensions
    {
        public static void SetEnabled<T>(this ICollection<T> behaviours, bool enabled) where T : Behaviour
        {
            foreach (var behaviour in behaviours)
                behaviour.enabled = enabled;
        }
    }
}