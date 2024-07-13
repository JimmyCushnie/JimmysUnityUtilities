using System.Collections;
using System.Collections.Generic;

namespace JimmysUnityUtilities.Collections
{
    /// <summary>
    /// An implementation of <see cref="IList{T}"/> that is thread-safe; it acquires a lock before executing any read or write operation.
    /// EXTREMELY IMPORTANT: If you iterate over this with foreach or directly with GetEnumerator, you absolutely must lock that operation with <see cref="__InternalListLock"/>
    /// </summary>
    public class LockedList<T> : IList<T>
    {
        private IList<T> InternalList = new List<T>();
        public readonly object __InternalListLock = new object();


        public IEnumerator<T> GetEnumerator() => InternalList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();


        public T this[int index]
        {
            get
            {
                lock (__InternalListLock)
                    return InternalList[index];
            }
            set
            {
                lock (__InternalListLock)
                    InternalList[index] = value;
            }
        }

        public int Count
        {
            get
            {
                lock (__InternalListLock)
                    return InternalList.Count;
            }
        }

        public bool IsReadOnly => InternalList.IsReadOnly;

        public void Add(T item)
        {
            lock (__InternalListLock)
                InternalList.Add(item);
        }

        public void Clear()
        {
            lock (__InternalListLock)
                InternalList.Clear();
        }

        public bool Contains(T item)
        {
            lock (__InternalListLock)
                return InternalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (__InternalListLock)
                InternalList.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            lock (__InternalListLock)
                return InternalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            lock (__InternalListLock)
                InternalList.Insert(index, item);
        }

        public bool Remove(T item)
        {
            lock (__InternalListLock)
                return InternalList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            lock (__InternalListLock)
                InternalList.RemoveAt(index);
        }
    }
}