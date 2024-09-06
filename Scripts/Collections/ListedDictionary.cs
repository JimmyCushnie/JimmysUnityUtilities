using System;
using System.Collections.Generic;

namespace JimmysUnityUtilities.Collections
{
    /// <summary>
    /// Like <see cref="Dictionary{TKey,TValue}"/>, but it can have multiple <see cref="TValue"/>s per <see cref="TKey"/>
    /// Somewhat limited functionality, for now
    /// </summary>
    public class ListedDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, List<TValue>> BackingDictionary = new Dictionary<TKey, List<TValue>>();

        public void AddValueAt(TKey key, TValue value)
        {
            if (BackingDictionary.TryGetValue(key, out var list))
            {
                list.Add(value);
            }
            else
            {
                BackingDictionary[key] = new List<TValue> { value };
            }
        }

        public IReadOnlyList<TValue> GetValuesAt(TKey key)
        {
            if (BackingDictionary.TryGetValue(key, out var list))
                return list;

            return Array.Empty<TValue>();
        }

        public bool ContainsValuesAt(TKey key)
            => BackingDictionary.TryGetValue(key, out var list) && list.IsNotEmpty();

        public void Clear()
        {
            BackingDictionary.Clear();
        }

        public void ClearValuesAt(TKey key)
        {
            BackingDictionary.Remove(key);
        }
    }
}