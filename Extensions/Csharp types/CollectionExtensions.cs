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
    public static class CollectionExtensions
    {
        public static T GetRandomElement<T>(this IReadOnlyList<T> list)
            => list[Random.Range(0, list.Count)];

        public static void SwapPositions<T>(this IList<T> list, int index1, int index2)
        {
            T t1 = list[index1];
            T t2 = list[index2];

            list[index1] = t2;
            list[index2] = t1;
        }
    }
}