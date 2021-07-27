using System.Collections.Generic;
using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace JimmysUnityUtilities
{
    public static class ComponentExtensions
    {
        // For some reason, you can use GetComponent from a component, but not AddComponent. This provides parity with GameObject methods.
        public static T AddComponent<T>(this Component co) where T : Component
            => co.gameObject.AddComponent<T>();

        public static T GetOrAddComponent<T>(this Component co) where T : Component
        {
            T component = co.GetComponent<T>();
            if (component == null) component = co.gameObject.AddComponent<T>();

            return component;
        }

        ///<summary> Returns true if the component existsed and was removed </summary>
        public static bool RemoveComponent<T>(this Component co) where T : Component
        {
            T component = co.GetComponent<T>();
            if (component != null)
            {
                Object.Destroy(component);
                return true;
            }
            return false;
        }

        ///<summary> Returns true if the component existsed and was removed </summary>
        public static bool RemoveComponentImmediate<T>(this Component co, bool allowDestroyingAssets = false) where T : Component
        {
            T component = co.GetComponent<T>();
            if (component != null)
            {
                Object.DestroyImmediate(component, allowDestroyingAssets);
                return true;
            }
            return false;
        }

        ///<summary> Returns true if the component existsed and was removed </summary>
        public static bool RemoveComponentInChildren<T>(this Component co) where T : Component
        {
            T component = co.GetComponentInChildren<T>();
            if (component != null)
            {
                Object.Destroy(component);
                return true;
            }
            return false;
        }

        public static RectTransform GetRectTransform(this Component co)
            => (RectTransform)co.transform;

        public static void SetEnabled<T>(this IEnumerable<T> components, bool value) where T : MonoBehaviour
        {
            foreach (var component in components)
                component.enabled = value;
        }


        // There's a built in TryGetComponent. This adds the same method in parent/children.
        public static bool TryGetComponentInChildren<T>(this Component co, out T component) where T : Component
        {
            component = co.GetComponentInChildren<T>();
            return component != null;
        }

        public static bool TryGetComponentInParent<T>(this Component co, out T component) where T : Component
        {
            component = co.GetComponentInParent<T>();
            return component != null;
        }
    }
}