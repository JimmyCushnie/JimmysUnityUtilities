using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    public class ObjectPoolUtility<T> where T : Component
    {
        public static ObjectPoolUtility<T> CreateBasic(GameObject prefab)
        {
            return new ObjectPoolUtility<T>
                (
                (Transform parent) 
                    => Object.Instantiate(prefab, parent).GetComponent<T>()
                );
        }

        protected System.Func<Transform, T> CreateNewObjectInParent;

        public ObjectPoolUtility(System.Func<Transform, T> createNewObjectInParent)
        {
            CreateNewObjectInParent = createNewObjectInParent;
        }

        protected Stack<T> InactiveObjectPool = new Stack<T>();
        public virtual T Get(Transform parent)
        {
            if (InactiveObjectPool.Count == 0)
            {
                return CreateNewObjectInParent(parent);
            }

            var item = InactiveObjectPool.Pop();
            item.transform.SetParent(parent, worldPositionStays: false);
            item.gameObject.SetActive(true);

            return item;
        }

        public virtual void Recycle(T item)
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(InactiveObjectParent, worldPositionStays: false);
            InactiveObjectPool.Push(item);
        }

        public void Recycle(IEnumerable<T> items)
        {
            foreach (var item in items)
                Recycle(item);
        }


        /// <summary>
        /// Call this when you're done using the pool to clean up inactive pool objects.
        /// </summary>
        public void DeleteAllInactiveObjects()
        {
            if (_InactiveObjectParent == null)
                return;

            Object.Destroy(_InactiveObjectParent);
            _InactiveObjectParent = null; // Set this explicity so that the "is null" check works.

            InactiveObjectPool.Clear();
        }
        

        private Transform _InactiveObjectParent;
        private Transform InactiveObjectParent
        {
            get
            {
                // Intentionally using "is null" instead of Unity's overloaded "== null" for performance when doing a lot of operations on a pool.
                // Unity's overload actually *does* have a significant performance impact, in terms of both CPU usage and GC. I checked with the profiler.
                // We should be safe here because the MegaParent is DontDestroyOnLoad and so should never be destroyed.
                if (_InactiveObjectParent is null)
                {
                    _InactiveObjectParent = new GameObject(typeof(T).Name).transform;
                    _InactiveObjectParent.SetParent(MegaPoolParentHolder.MegaParent);
                    _InactiveObjectParent.gameObject.SetActive(false);
                }

                return _InactiveObjectParent;
            }
        }
    }
}
