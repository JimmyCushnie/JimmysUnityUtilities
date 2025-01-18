using System;
using System.Collections.Generic;

namespace JimmysUnityUtilities.Collections
{
    // This is similar to TwoWayDictionary, but pairs are of the same type rather than different types.
    
    /// <summary>
    /// Maps pairs of <typeparam name="T"></typeparam>.
    /// In each pair, one is called A and the other B.
    /// You can use one member of the pair to look up the other. I.e. you can get the B from the A or the A from the B.
    /// Can't contain two identical As or two identical Bs.
    /// </summary>
    public class BiMap<T>
    {
        readonly Dictionary<T, T> _AtoB;
        readonly Dictionary<T, T> _BtoA;

        public IReadOnlyDictionary<T, T> AtoB => _AtoB;
        public IReadOnlyDictionary<T, T> BtoA => _BtoA;

        public IEnumerable<T> SetA => AtoB.Keys;
        public IEnumerable<T> SetB => BtoA.Keys;


        public BiMap()
        {
            _AtoB = new Dictionary<T, T>();
            _BtoA = new Dictionary<T, T>();
        }

        public BiMap(int capacity)
        {
            _AtoB = new Dictionary<T, T>(capacity);
            _BtoA = new Dictionary<T, T>(capacity);
        }

        public void AddPair(T a, T b)
        {
            if (ContainsA(a))
                throw new ArgumentException("Already contains this A!");

            if (ContainsB(b))
                throw new ArgumentException("Already contains this B!");
            
            _AtoB.Add(a, b);
            _BtoA.Add(b, a);
        }

        public T GetBfromA(T a) => _AtoB[a];
        public bool TryGetBfromA(T a, out T b) => _AtoB.TryGetValue(a, out b);

        public T GetAfromB(T b) => _BtoA[b];
        public bool TryGetAfromB(T b, out T a) => _BtoA.TryGetValue(b, out a);

        public int Count => _AtoB.Count;

        public bool ContainsA(T a) => _AtoB.ContainsKey(a);
        public bool ContainsB(T b) => _BtoA.ContainsKey(b);

        public bool RemovePairByA(T a)
        {
            if (!ContainsA(a))
                return false;

            var b = _AtoB[a];

            _BtoA.Remove(b);
            _AtoB.Remove(a);

            return true;
        }

        public bool RemovePairByB(T b)
        {
            if (!ContainsB(b))
                return false;

            var a = _BtoA[b];

            _AtoB.Remove(a);
            _BtoA.Remove(b);

            return true;
        }

        public void Clear()
        {
            _AtoB.Clear();
            _BtoA.Clear();
        }
    }
}
