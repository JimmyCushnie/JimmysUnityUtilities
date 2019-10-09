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
    public static class ColliderExtensions
    {
        public static void SetEnabled(this ICollection<Collider> colliders, bool value)
        {
            foreach (var collider in colliders)
                collider.enabled = value;
        }
    }
}