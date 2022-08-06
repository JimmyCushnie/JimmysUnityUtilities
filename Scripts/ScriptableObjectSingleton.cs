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


        /// <summary> Called once in the application lifetime. </summary>
        protected virtual void Initialize() { }

        // You might be tempted to use ScriptableObject.Awake(), but that method is only called when the SO is created for the first time. OnEnable is called at application startup
        // and also some other times? Unity is fucking annoying
        private void OnEnable()
        {
            if (InitializeCalled)
                return;

            InitializeCalled = true;
            Initialize();
        }
        static bool InitializeCalled = false;
    }
}