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
    public static class TransformExtensions
    {
        public static void DestroyAllChildren(this Transform t)
            => t.DestroyChildren(0, t.childCount - 1);

        public static void DestroyChildren(this Transform t, int StartDestroyingIndex, int EndDestroyingIndex)
        {
            for (int i = StartDestroyingIndex; i <= EndDestroyingIndex; i++)
                Object.Destroy(t.GetChild(i).gameObject);
        }

        public static void DestroyActiveChildren(this Transform t)
        {
            for (int i = 0; i <= t.childCount - 1; i++)
            {
                var gameObject = t.GetChild(i).gameObject;
                if (gameObject.activeSelf)
                    Object.Destroy(gameObject);
            }
        }

        public static void DestroyInactiveChildren(this Transform t)
        {
            for (int i = 0; i <= t.childCount - 1; i++)
            {
                var gameObject = t.GetChild(i).gameObject;
                if (!gameObject.activeSelf)
                    Object.Destroy(gameObject);
            }
        }

        public static void SortChildrenAlphabetically(this Transform t)
        {
            var childNames = new List<string>(t.childCount);

            for (int i = 0; i < t.childCount; i++)
                childNames.Add(t.GetChild(i).gameObject.name);

            childNames.Sort();

            for (int i = 0; i < childNames.Count; i++)
                t.Find(childNames[i]).SetAsLastSibling();
        }
    }
}