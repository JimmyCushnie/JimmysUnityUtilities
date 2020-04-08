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

        public ObjectPoolUtility (System.Func<Transform, T> createNewObjectInParent)
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
        

        private Transform _InactiveObjectParent;
        private Transform InactiveObjectParent
        {
            get
            {
                if (_InactiveObjectParent == null)
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
