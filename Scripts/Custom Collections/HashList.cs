using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JimmysUnityUtilities
{
    /// <summary>
    /// A hybrid of List and HashSet that has the best of both: a specific order and fast iteration performance, but also fast Contains() checks.
    /// Cannot have duplicate items.
    /// </summary>
    public class HashList<T> : 
        IList<T>,
        IList,
        IReadOnlyList<T>,
        ISet<T>
    {
        readonly List<T> _List;
        readonly HashSet<T> _HashSet;
        
        public HashList()
        {
            _List = new List<T>();
            _HashSet = new HashSet<T>();
        }
        public HashList(int capacity)
        {
            _List = new List<T>(capacity);
            _HashSet = new HashSet<T>(); // todo set capacity once we're up to modern dotnet
        }
        public HashList(IEnumerable<T> collection)
        {
            _List = new List<T>(collection);
            _HashSet = new HashSet<T>(collection);
        }
        
        
        public int Count => _List.Count;
        
        public IEnumerator<T> GetEnumerator() => _List.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_List).GetEnumerator();

        public bool Add(T item)
        {
            if (_HashSet.Add(item))
            {
                _List.Add(item);
                return true;
            }
        
            return false;
        }
        public int Add(object value)
        {
            if (!(value is T item)) // todo use is not pattern when we c# 9
                throw new ArgumentException("Value was wrong type!", nameof(value));

            if (Add(item))
                return Count - 1;

            return -1;
        }
        void ICollection<T>.Add(T item) => Add(item);

        public bool Remove(T item)
        {
            if (_HashSet.Remove(item))
            {
                _List.Remove(item);
                return true;
            }

            return false;
        }
        public void Remove(object value)
        {
            if (!(value is T item)) // todo use is not pattern when we c# 9
                throw new ArgumentException("Value was wrong type!", nameof(value));

            Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (index < this.FirstIndex() || index > this.LastIndex())
                throw new ArgumentOutOfRangeException(nameof(index));

            T item = this[index];
            _HashSet.Remove(item);
            _List.RemoveAt(index);
        }

        public int IndexOf(T item) => _List.IndexOf(item);
        public int IndexOf(object value)
        {
            if (!(value is T item)) // todo use is not pattern when we c# 9
                throw new ArgumentException("Value was wrong type!", nameof(value));

            return IndexOf(item);
        }
        
        public bool Contains(T item) => _HashSet.Contains(item);
        public bool Contains(object value)
        {
            if (!(value is T item)) // todo use is not pattern when we c# 9
                throw new ArgumentException("Value was wrong type!", nameof(value));

            return Contains(item);
        }
        
        public bool Insert(int index, T item)
        {
            if (Contains(item))
                return false;
            
            _List.Insert(index, item);
            _HashSet.Add(item);
            return true;
        }
        public void Insert(int index, object value)
        {
            if (!(value is T item)) // todo use is not pattern when we c# 9
                throw new ArgumentException("Value was wrong type!", nameof(value));

            Insert(index, item);
        }
        void IList<T>.Insert(int index, T item) => Insert(index, item);
        
        public void Clear()
        {
            _List.Clear();
            _HashSet.Clear();
        }

        public T this[int index]
        {
            get => _List[index];
            set => _List[index] = value;
        }
        object IList.this[int index]
        {
            get => this[index];
            set
            {
                if (!(value is T item)) // todo use is not pattern when we c# 9
                    throw new ArgumentException("Value was wrong type!", nameof(value));
                
                this[index] = item;
            }
        }

        
        public void CopyTo(T[] array, int arrayIndex) => _List.CopyTo(array, arrayIndex);
        public void CopyTo(Array array, int index) => ((ICollection)_List).CopyTo(array, index);
        
        public void ExceptWith(IEnumerable<T> other)
        {
            foreach (T obj in other)
                this.Remove(obj);
        }
        public void IntersectWith(IEnumerable<T> other)
        {
            foreach (T obj in this.ToArray())
            {
                if (!other.Contains(obj))
                    Remove(obj);
            }
        }
        public void SymmetricExceptWith(IEnumerable<T> other)
        {      
            foreach (T obj in other)
            {
                if (!this.Remove(obj))
                    this.Add(obj);
            }
        }
        public void UnionWith(IEnumerable<T> other)
        {
            _HashSet.UnionWith(other);
            foreach (T obj in other)
                Add(obj);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other) => _HashSet.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<T> other) => _HashSet.IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<T> other) => _HashSet.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<T> other) => _HashSet.IsSupersetOf(other);
        public bool Overlaps(IEnumerable<T> other) => _HashSet.Overlaps(other);
        public bool SetEquals(IEnumerable<T> other) => _HashSet.SetEquals(other);
        
        public bool IsFixedSize => false;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot { get; } = new object();
    }
}
