using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace JimmysUnityUtilities
{
    public class Dispatcher : MonoBehaviour
    {
        private static Dispatcher _Instance;
        private static Dispatcher Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
                    DontDestroyOnLoad(_Instance);
                }

                return _Instance;
            }
        }

        private Thread MainThread;
        private ConcurrentQueue<(Action Action, ManualResetEventSlim Event)> Queue = new ConcurrentQueue<(Action, ManualResetEventSlim)>();

        private bool OnMainThread => Thread.CurrentThread == MainThread;

        private void Awake()
        {
            MainThread = Thread.CurrentThread;
        }

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


        public static void InvokeAsync(Action action)
        {
            if (Instance.OnMainThread)
                action();
            else
                Instance.Queue.Enqueue((action, null));
        }

        public static void Invoke(Action action)
        {
            if (Instance.OnMainThread)
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
    }
}
