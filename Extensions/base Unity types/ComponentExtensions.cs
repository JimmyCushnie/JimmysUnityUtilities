using System.Collections.Generic;
using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace JimmysUnityUtilities
{
    public static class ComponentExtensions
    {
        // for some reason, you can use GetComponent from a component, but not AddComponent. This provides parity with GameObject methods.
        public static T AddComponent<T>(this Component co) where T : Component
            => co.gameObject.AddComponent<T>();

        public static T GetOrAddComponent<T>(this Component co) where T : Component
        {
            T component = co.GetComponent<T>();
            if (component == null) component = co.gameObject.AddComponent<T>();

            return component;
        }

        ///<summary> returns true if the component existsed and was removed </summary>
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

        ///<summary> returns true if the component existsed and was removed </summary>
        public static bool RemoveComponentImmediate<T>(this Component co) where T : Component
        {
            T component = co.GetComponent<T>();
            if (component != null)
            {
                Object.DestroyImmediate(component);
                return true;
            }
            return false;
        }

        public static RectTransform GetRectTransform(this Component co)
            => (RectTransform)co.transform;

        public static void SetEnabled(this ICollection<MonoBehaviour> components, bool value)
        {
            foreach (var component in components)
                component.enabled = value;
        }
    }
}