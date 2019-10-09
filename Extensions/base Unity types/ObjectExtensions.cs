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
    public static class ObjectExtensions
    {
        /// <summary> destroys all UnityEngine.Objects in an array of them. </summary>
        public static void DestroyAll<T>(this T[] array) where T : Object
        {
            for (int i = 0; i < array.Length; i++)
                Object.Destroy(array[i]);
        }
    }
}