using System;
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


        public static IEnumerable<T> Subsequence<T>(this IReadOnlyList<T> list, int startIndex)
        {
            if (startIndex < 0)
                throw new ArgumentException($"{nameof(startIndex)} must not be less than 0");

            if (startIndex >= list.Count)
                throw new ArgumentException($"{nameof(startIndex)} must not be greater than the length of {nameof(list)}");

            return list.Subsequence(startIndex, list.Count - startIndex);
        }

        public static IEnumerable<T> Subsequence<T>(this IEnumerable<T> collection, int startIndex, int length)
        {
            return collection.Skip(startIndex).Take(length);
        }


        public static IEnumerable<IEnumerable<T>> GetAllPossibleSubsets<T>(this IEnumerable<T> set)
        {
            var array = set.ToArray();
            int max = 2.ToThePowerOf(array.Length);

            for (int i = 0; i < max; i++)
            {
                var subset = new List<T>();
                uint bitmaskFuckery = 0;

                while (bitmaskFuckery < array.Length)
                {
                    if ((i & (1u << (int)bitmaskFuckery)) > 0)
                    {
                        subset.Add(array[(int)bitmaskFuckery]);
                    }

                    bitmaskFuckery++;
                }

                yield return subset;
            }
        }

        public static bool IsSubsetOf<T>(this IEnumerable<T> subset, IEnumerable<T> set)
            => subset.Except(set).IsEmpty();
    }
}