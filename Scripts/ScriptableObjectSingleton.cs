using UnityEngine;

namespace JimmysUnityUtilities
{
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _Instance = null;
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = Resources.Load<T>(typeof(T).Name);

                return _Instance;
            }
        }
    }
}