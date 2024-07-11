using System;
using System.Collections.Generic;
using System.Linq;

namespace JimmysUnityUtilities
{
    public class TwoWayDictionary<T1, T2>
    {
        readonly Dictionary<T1, T2> _Forwards = new Dictionary<T1, T2>();
        readonly Dictionary<T2, T1> _Backwards = new Dictionary<T2, T1>();

        public IReadOnlyDictionary<T1, T2> Forwards => _Forwards;
        public IReadOnlyDictionary<T2, T1> Backwards => _Backwards;

        public IEnumerable<T1> Set1 => Forwards.Keys;
        public IEnumerable<T2> Set2 => Backwards.Keys;


        public TwoWayDictionary()
        {
            _Forwards = new Dictionary<T1, T2>();
            _Backwards = new Dictionary<T2, T1>();
        }

        public TwoWayDictionary(int capacity)
        {
            _Forwards = new Dictionary<T1, T2>(capacity);
            _Backwards = new Dictionary<T2, T1>(capacity);
        }

        public TwoWayDictionary(Dictionary<T1, T2> initial)
        {
            _Forwards = initial;
            _Backwards = initial.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public TwoWayDictionary(Dictionary<T2, T1> initial)
        {
            _Backwards = initial;
            _Forwards = initial.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public void Add(T1 key, T2 value)
        {
            if (ContainsKey(key))
                throw new ArgumentException("Already contains this key!");

            this[key] = value;
        }

        public void Add(T2 key, T1 value)
        {
            if (ContainsKey(key))
                throw new ArgumentException("Already contains this key!");

            this[key] = value;
        }

        public T1 this[T2 index]
        {
            get => _Backwards[index];
            set
            {
                if (_Backwards.TryGetValue(index, out var removeThis))
                    _Forwards.Remove(removeThis);

                _Backwards[index] = value;
                _Forwards[value] = index;
            }
        }

        public T2 this[T1 index]
        {
            get => _Forwards[index];
            set
            {
                if (_Forwards.TryGetValue(index, out var removeThis))
                    _Backwards.Remove(removeThis);

                _Forwards[index] = value;
                _Backwards[value] = index;
            }
        }

        public int Count => _Forwards.Count;

        public bool ContainsKey(T1 key) => _Forwards.ContainsKey(key);
        public bool ContainsKey(T2 key) => _Backwards.ContainsKey(key);

        public bool Remove(T1 item)
        {
            if (!ContainsKey(item))
                return false;

            var t2 = _Forwards[item];

            _Backwards.Remove(t2);
            _Forwards.Remove(item);

            return true;
        }

        public bool Remove(T2 item)
        {
            if (!ContainsKey(item))
                return false;

            var t1 = _Backwards[item];

            _Forwards.Remove(t1);
            _Backwards.Remove(item);

            return true;
        }

        public void Clear()
        {
            _Forwards.Clear();
            _Backwards.Clear();
        }

        public bool TryGetValue(T1 key, out T2 value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetValue(T2 key, out T1 value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }

            value = default;
            return false;
        }
    }
}
