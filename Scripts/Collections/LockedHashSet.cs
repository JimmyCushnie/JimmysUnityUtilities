using System.Collections;
using System.Collections.Generic;

namespace JimmysUnityUtilities.Collections
{
    /// <summary>
    /// An implementation of <see cref="ISet{T}"/> that is thread-safe; it acquires a lock before executing any read or write operation.
    /// EXTREMELY IMPORTANT: If you iterate over this with foreach or directly with GetEnumerator, you absolutely must lock that operation with <see cref="__InternalHashSetLock"/>
    /// </summary>
    public class LockedHashSet<T> : ISet<T>
    {
        private ISet<T> InternalHashSet = new HashSet<T>();
        public readonly object __InternalHashSetLock = new object();


        public IEnumerator<T> GetEnumerator() => InternalHashSet.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();


        public int Count
        {
            get
            {
                lock (__InternalHashSetLock)
                    return InternalHashSet.Count;
            }
        }

        public bool IsReadOnly => InternalHashSet.IsReadOnly;

        public bool Add(T item)
        {
            lock (__InternalHashSetLock)
                return InternalHashSet.Add(item);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            lock (__InternalHashSetLock)
                InternalHashSet.ExceptWith(other);
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            lock (__InternalHashSetLock)
                InternalHashSet.IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            lock (__InternalHashSetLock)
                return InternalHashSet.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            lock (__InternalHashSetLock)
                return InternalHashSet.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            lock (__InternalHashSetLock)
                return InternalHashSet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            lock (__InternalHashSetLock)
                return InternalHashSet.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            lock (__InternalHashSetLock)
                return InternalHashSet.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            lock (__InternalHashSetLock)
                return InternalHashSet.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            lock (__InternalHashSetLock)
                InternalHashSet.SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            lock (__InternalHashSetLock)
                InternalHashSet.UnionWith(other);
        }

        void ICollection<T>.Add(T item)
        {
            lock (__InternalHashSetLock)
                InternalHashSet.Add(item);
        }

        public void Clear()
        {
            lock (__InternalHashSetLock)
                InternalHashSet.Clear();
        }

        public bool Contains(T item)
        {
            lock (__InternalHashSetLock)
                return InternalHashSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (__InternalHashSetLock)
                InternalHashSet.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            lock (__InternalHashSetLock)
                return InternalHashSet.Remove(item);
        }
    }
}