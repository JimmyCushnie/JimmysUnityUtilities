using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;

namespace JimmysUnityUtilities
{
    /// <summary>
    /// Utility for calling functions on the main Unity thread from multithreaded code.
    /// Note that to use <see cref="Dispatcher"/>, you must first call <see cref="Dispatcher.EnsureInitialized"/>, from the main Unity thread.
    /// </summary>
    public class Dispatcher : MonoBehaviour
    {
        /// <summary>
        /// You must call this, exactly once, from the main Unity thread before you use the other methods in this class.
        /// It is recommended that you do this on application startup.
        /// </summary>
        public static void EnsureInitialized()
        {
            if (Initialized)
                return;

            try
            {
                _Instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
                DontDestroyOnLoad(_Instance);
            }
            catch (UnityException)
            {
                throw new Exception($"{nameof(EnsureInitialized)} must be called from the main Unity thread");
            }

            MainThreadID = Thread.CurrentThread.ManagedThreadId;
        }

        private static Dispatcher _Instance;
        private static Dispatcher Instance
        {
            get
            {
                if (_Instance == null)
                    throw new Exception($"You cannot use {nameof(Dispatcher)} until you've called {nameof(EnsureInitialized)}");

                return _Instance;
            }
        }
        public static bool Initialized => _Instance != null;

        private static int MainThreadID;
        private static bool OnMainThread => Thread.CurrentThread.ManagedThreadId == MainThreadID;


        /// <summary>
        /// Runs some code on the main thread. If called from a non-main thread, will return immediately, before <paramref name="action"/> has finished running.
        /// </summary>
        public static void InvokeAsync(Action action)
        {
            if (!Initialized)
                throw new Exception($"You cannot use this method until you've called {nameof(EnsureInitialized)}");


            if (OnMainThread)
                action();
            else
                Instance.Queue.Enqueue((action, null));
        }

        /// <summary>
        /// Runs some code on the main thread. Will wait for <paramref name="action"/> to finish running before it returns.
        /// </summary>
        public static void Invoke(Action action)
        {
            if (!Initialized)
                throw new Exception($"You cannot use this method until you've called {nameof(EnsureInitialized)}");


            if (OnMainThread)
            {
                action();
                return;
            }

            using (var ev = new ManualResetEventSlim())
            {
                Instance.Queue.Enqueue((action, ev));

                ev.Wait();
            }
        }


        private ConcurrentQueue<(Action Action, ManualResetEventSlim Event)> Queue = new ConcurrentQueue<(Action, ManualResetEventSlim)>();
        private void Update()
        {
            while (Queue.Count > 0)
            {
                if (!Queue.TryDequeue(out var item))
                    continue;

                item.Action();
                item.Event?.Set();
            }
        }

    }
}
