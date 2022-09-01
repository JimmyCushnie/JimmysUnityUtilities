using System;
using System.Collections.Generic;
using System.Linq;
using JimmysUnityUtilities.Random;

namespace JimmysUnityUtilities
{
    public static class CollectionExtensions
    {
        public static T GetRandomElement<T>(this IReadOnlyList<T> list) => JRandom.Shared.RandomElementOf(list);

        public static void SwapPositions<T>(this IList<T> list, int index1, int index2)
        {
            if (index1 < 0 || index1 >= list.Count)
                throw new IndexOutOfRangeException();

            if (index2 < 0 || index2 >= list.Count)
                throw new IndexOutOfRangeException();

            if (index1 == index2)
                return;


            T t1 = list[index1];
            T t2 = list[index2];

            list[index1] = t2;
            list[index2] = t1;
        }

        public static void MoveItem<T>(this IList<T> list, int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(oldIndex));

            if (newIndex < 0 || newIndex >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(newIndex));

            if (oldIndex == newIndex)
                return;

            T item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);
        }

        /// <summary>
        /// Removes an item in 0(1) time by swapping it with the last item in the list, then removing it from there.
        /// In almost all situations this will be faster than <see cref="IList{T}.RemoveAt(int)"/>, but it will mess up
        /// the order of the list, so only use it where order is unimportant.
        /// </summary>
        public static void FastRemoveAt<T>(this IList<T> list, int index)
        {
            int lastIndex = list.Count - 1;

            list.SwapPositions(index, lastIndex);
            list.RemoveAt(lastIndex);
        }

        public static void RemoveAllItemsAtAndAfter<T>(this IList<T> list, int startRemovingAtThisIndex)
        {
            if (startRemovingAtThisIndex < 0)
                throw new ArgumentOutOfRangeException($"{nameof(startRemovingAtThisIndex)} must be at least 0");

            if (startRemovingAtThisIndex >= list.Count)
                throw new ArgumentOutOfRangeException($"{nameof(startRemovingAtThisIndex)} must be less than the list's count");


            int maxListCount = startRemovingAtThisIndex;
            while (list.Count > maxListCount)
                list.RemoveAt(list.Count - 1);
        }

        public static void EnsureCapacity<T>(this List<T> list, int minimumCapacity)
        {
            if (list.Capacity < minimumCapacity)
                list.Capacity = minimumCapacity;
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


        /// <summary>
        /// Swaps a dictionary so that the keys become the values and the values become the keys.
        /// If multiple keys in the original contain the same value, some information will be lost. To preserve it, use <see cref="InvertWithList{T1, T2}(IReadOnlyDictionary{T1, T2})"/>
        /// </summary>
        public static Dictionary<T2, T1> Invert<T1, T2>(this IReadOnlyDictionary<T1, T2> dictionary)
        {
            return dictionary
                .GroupBy(p => p.Value)
                .ToDictionary(g => g.Key, g => g.Select(pp => pp.Key).First());
        }

        /// <summary>
        /// Swaps a dictionary so that the keys become the values and the values become the keys.
        /// </summary>
        public static Dictionary<T2, List<T1>> InvertWithList<T1, T2>(this IReadOnlyDictionary<T1, T2> dictionary)
        {
            return dictionary
                .GroupBy(p => p.Value)
                .ToDictionary(g => g.Key, g => g.Select(pp => pp.Key).ToList());
        }

        /// <summary>
        /// Removes all entries from a dictionary with the specified value.
        /// </summary>
        public static void RemoveAllEntriesWithValue<T1, T2>(this IDictionary<T1, T2> dictionary, T2 value)
        {
            foreach (var matchingEntry in dictionary.Where(kvp => kvp.Value.Equals(value)).ToArray())
                dictionary.Remove(matchingEntry.Key);
        }


        public static bool ContainsIndex<T>(this IReadOnlyList<T> list, int index)
        {
            if (index < 0)
                return false;

            return index < list.Count;
        }

        public static bool ContainsIndex<T>(this IReadOnlyList<T> list, int index, out T valueAtIndex)
        {
            if (!list.ContainsIndex(index))
            {
                valueAtIndex = default;
                return false;
            }

            valueAtIndex = list[index];
            return true;
        }

        public static bool ContainsIndex(this Array array, int index, int dimension)
        {
            if (index < 0)
                return false;

            return index < array.GetLength(dimension);
        }


        public static void ExpandIfNecessaryUntilContainsIndex<T>(this IList<T> list, int index)
        {
            if (index < 0)
                throw new IndexOutOfRangeException();

            while (list.Count <= index)
                list.Add(default);
        }


        public static int FirstIndex<T>(this IReadOnlyList<T> list)
        {
            if (list.Count == 0)
                throw new Exception("List has no elements, and therefore no first index");

            return 0;
        }
        public static int LastIndex<T>(this IReadOnlyList<T> list)
        {
            if (list.Count == 0)
                throw new Exception("List has no elements, and therefore no last index");

            return list.Count - 1;
        }

        public static T PopFromEnd<T>(this IList<T> list)
        {
            if (list.Count == 0)
                throw new Exception("List has no elements; cannot pop");


            int lastIndex = list.Count - 1;

            T last = list[lastIndex];
            list.RemoveAt(lastIndex);
            return last;
        }
        public static T PopFromStart<T>(this IList<T> list)
        {
            if (list.Count == 0)
                throw new Exception("List has no elements; cannot pop");


            T first = list[0];
            list.RemoveAt(0);
            return first;
        }


        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action.Invoke(item);
        }


        /// <summary>
        /// Efficiently convert an array of one type to an array of a different type.
        /// </summary>
        public static Tdestination[] Convert<Tsource, Tdestination>(this Tsource[] sourceArray, Func<Tsource, Tdestination> conversionFunction)
        {
            if (sourceArray == null)
                return null;


            var convertedArray = new Tdestination[sourceArray.Length];
            for (int i = 0; i < sourceArray.Length; i++)
            {
                convertedArray[i] = conversionFunction(sourceArray[i]);
            }

            return convertedArray;
        }

        /// <summary>
        /// Like <see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>, but it accepts null parameters, and treats them the same as an empty enumerable.
        /// </summary>
        public static bool HasTheSameContentsAs<T>(this IEnumerable<T> enumerable1, IEnumerable<T> enumerable2)
        {
            return enumerable1.OrEmptyIfNull().SequenceEqual(enumerable2.OrEmptyIfNull());
        }

        public static bool HasTheSameContentsAs_IgnoringOrder<T>(this IEnumerable<T> enumerable1, IEnumerable<T> enumerable2)
        {
            enumerable1 = enumerable1.OrEmptyIfNull().OrderBy(x => x);
            enumerable2 = enumerable2.OrEmptyIfNull().OrderBy(x => x);

            return enumerable1.SequenceEqual(enumerable2);
        }

        public static bool AllElementsAreTheSame<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.GroupBy(o => o).Count() == 1;
        }

        public static bool AllElementsHaveTheSameProperty<TSource, TProperty>(this IEnumerable<TSource> sourceEnumerable, Func<TSource, TProperty> getProperty)
        {
            return sourceEnumerable.GroupBy(getProperty).Count() == 1;
        }


        public static bool HasTheSameKeysAndValuesAs<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dicA, IReadOnlyDictionary<TKey, TValue> dicB, IEqualityComparer<TValue> valueComparer = null)
        {
            if (dicA.Count != dicB.Count)
                return false;

            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            foreach (var key in dicA.Keys)
            {
                if (!dicB.ContainsKey(key))
                    return false;

                if (!valueComparer.Equals(dicB[key], dicA[key]))
                    return false;
            }

            return true;
        }



        public static T[] Duplicate<T>(this T[] sourceArray)
        {
            if (sourceArray == null)
                return null;


            var duplicateArray = new T[sourceArray.Length];
            Array.Copy(sourceArray, duplicateArray, sourceArray.Length);

            return duplicateArray;            
        }

        public static T[] Duplicate<T>(this IReadOnlyList<T> sourceList)
        {
            if (sourceList == null)
                return null;


            var duplicateArray = new T[sourceList.Count];
            for (int i = 0; i < sourceList.Count; i++)
                duplicateArray[i] = sourceList[i];

            return duplicateArray;
        }


        public static bool ContainsSequence<T>(this T[] source, T[] sequence)
        {
            for (int i = 0; i <= source.Length - sequence.Length; i++)
            {
                if (source.Skip(i).Take(sequence.Length).SequenceEqual(sequence))
                    return true;
            }

            return false;
        }



        /// <summary>
        /// Does <see cref="List{T}.Contains(T)"/> with a null check.
        /// </summary>
        public static bool IsNotNullAndContains<T>(this List<T> list, T item)
            => list != null && list.Contains(item);
    }
}