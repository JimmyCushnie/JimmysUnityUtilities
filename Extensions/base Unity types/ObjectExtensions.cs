using Object = UnityEngine.Object;

namespace JimmysUnityUtilities
{
    public static class ObjectExtensions
    {
        /// <summary> destroys all UnityEngine.Objects in an array of them. </summary>
        public static void DestroyAll<T>(this T[] array) where T : Object
        {
            for (int i = 0; i < array.Length; i++)
                Object.Destroy(array[i]);
        }
    }
}