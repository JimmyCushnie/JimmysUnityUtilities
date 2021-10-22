using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public T Get() => Get(null);
        public virtual T Get(Transform parent)
        {
            if (InactiveObjectPool.Count == 0)
            {
                return CreateNewObjectInParent(parent);
            }

            var item = InactiveObjectPool.Pop();
            item.transform.SetParent(parent, worldPositionStays: false);
            item.gameObject.SetActive(true);

            if (parent == null)
            {
                // If a new object is created with no parent (i.e. using UnityEngine.Object.Instantiate), it will be in the main scene.
                // However, if a pooled object is moved to no parent, it will stay in the DontDestroyOnLoad scene.
                // This makes the scene of items consistent for both new items and pooled items.
                // One example for why this might matter is that users expect the items to be destroyed by Unity when changing scenes.
                SceneManager.MoveGameObjectToScene(item.gameObject, SceneManager.GetActiveScene());
            }

            return item;
        }

        public virtual void Recycle(T item)
        {
            if (item == null) // In case the item was destroyed before being recycled (i.e, the scene it was in got unloaded)
                return;

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
