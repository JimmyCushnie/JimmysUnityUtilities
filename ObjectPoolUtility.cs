using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tung
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

        private System.Func<Transform, T> CreateNewObjectInParent;

        public ObjectPoolUtility (System.Func<Transform, T> createNewObjectInParent)
        {
            CreateNewObjectInParent = createNewObjectInParent;
        }

        private Stack<T> InactiveObjectPool = new Stack<T>();
        public T Get(Transform parent)
        {
            if (InactiveObjectPool.Count == 0)
            {
                return CreateNewObjectInParent(parent);
            }

            var item = InactiveObjectPool.Pop();
            item.transform.SetParent(parent);
            return item;
        }

        public void Recycle(T item, bool worldPositionStays = false)
        {
            item.transform.SetParent(InactiveObjectParent, worldPositionStays);
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
    internal static class MegaPoolParentHolder
    {
        public static Transform _MegaParent;
        public static Transform MegaParent
        {
            get
            {
                if (_MegaParent == null)
                {
                    _MegaParent = new GameObject("pooled objects parent").transform;
                    _MegaParent.gameObject.SetActive(false);
                }

                return _MegaParent;
            }
        }
    }
}
