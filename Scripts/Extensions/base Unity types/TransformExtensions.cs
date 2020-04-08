using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JimmysUnityUtilities
{
    public static class TransformExtensions
    {
        public static void DestroyAllChildren(this Transform t)
            => t.DestroyChildren(0, t.childCount - 1);

        public static void DestroyChildrenAfterIndex(this Transform t, int startDestroyingIndex)
            => t.DestroyChildren(startDestroyingIndex, t.childCount - 1);

        public static void DestroyChildren(this Transform t, int startDestroyingIndex, int endDestroyingIndex)
        {
            // this works because Object.Destroy() doesn't actually take effect until the end of the frame
            for (int i = startDestroyingIndex; i <= endDestroyingIndex; i++)
                Object.Destroy(t.GetChild(i).gameObject);
        }

        public static void DestroyAllChildrenImmediate(this Transform t)
            => t.DestroyChildrenImmediate(0, t.childCount - 1);

        public static void DestroyChildrenAfterIndexImmediate(this Transform t, int startDestroyingIndex)
            => t.DestroyChildrenImmediate(startDestroyingIndex, t.childCount - 1);

        public static void DestroyChildrenImmediate(this Transform t, int startDestroyingIndex, int endDestroyingIndex)
        {
            for (int i = startDestroyingIndex; i <= endDestroyingIndex; i++)
                Object.DestroyImmediate(t.GetChild(startDestroyingIndex).gameObject);
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