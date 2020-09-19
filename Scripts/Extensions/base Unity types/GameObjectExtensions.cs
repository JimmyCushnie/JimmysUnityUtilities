using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace JimmysUnityUtilities
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null) component = go.AddComponent<T>();

            return component;
        }

        ///<summary> returns true if the component existsed and was removed </summary>
        public static bool RemoveComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component != null)
            {
                Object.Destroy(component);
                return true;
            }
            return false;
        }

        public static bool RemoveLastComponent<T>(this GameObject go) where T : Component
        {
            T[] components = go.GetComponents<T>();
            T component = components[components.Length - 1];

            if (component != null)
            {
                Object.Destroy(component);
                return true;
            }
            return false;
        }

        ///<summary> returns true if the component existsed and was removed </summary>
        public static bool RemoveComponentImmediate<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component != null)
            {
                Object.DestroyImmediate(component);
                return true;
            }
            return false;
        }

        /// <summary> Sets the layer of the object as well as all of its children. </summary>
        public static void SetLayerRecursively(this GameObject go, int layer)
        {
            go.layer = layer;

            for (int i = 0; i < go.transform.childCount; i++)
                go.transform.GetChild(i).gameObject.SetLayerRecursively(layer);
        }

        public static RectTransform GetRectTransform(this GameObject go)
        {
            return (RectTransform)go.transform;
        }
    }
}