using UnityEngine;
using System.Collections.Generic;

namespace JimmysUnityUtilities
{
    public class TrackedObjectPoolUtility<T> : ObjectPoolUtility<T> where T : Component
    {
        public TrackedObjectPoolUtility(System.Func<Transform, T> createNewObjectInParent) : base(createNewObjectInParent) 
        {
        }

        public static new TrackedObjectPoolUtility<T> CreateBasic(GameObject prefab)
        {
            return new TrackedObjectPoolUtility<T>
                (
                (Transform parent)
                    => Object.Instantiate(prefab, parent).GetComponent<T>()
                );
        }

        protected List<T> _ActiveObjects = new List<T>();
        public IReadOnlyList<T> ActiveObjects => _ActiveObjects;

        public override T Get(Transform parent)
        {
            T item = base.Get(parent);
            _ActiveObjects.Add(item);
            return item;
        }

        public override void Recycle(T item)
        {
            base.Recycle(item);
            _ActiveObjects.Remove(item);
        }

        public void RecycleAllActiveItems()
        {
            foreach (var item in _ActiveObjects.ToArray())
                Recycle(item);
        }
    }
}
