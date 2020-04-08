using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public static void MoveItem<T>(this IList<T> list, int oldIndex, int newIndex)
        {
            T item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);
        }

        /// <summary>
        /// Great for foreach loops on collections that might be null
        /// </summary>
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
            => !collection.Any();
    }
}