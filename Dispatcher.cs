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
        private static Dispatcher Current;

        private Thread MainThread;
        private ConcurrentQueue<(Action Action, ManualResetEventSlim Event)> Queue = new ConcurrentQueue<(Action, ManualResetEventSlim)>();

        private bool OnMainThread => Thread.CurrentThread == MainThread;

        public Dispatcher()
        {
            Current = this;
        }

        private void Awake()
        {
            MainThread = Thread.CurrentThread;
        }

        public static void InvokeAsync(Action action)
        {
            if (Current.OnMainThread)
                action();
            else
                Current.Queue.Enqueue((action, null));
        }

        public static void Invoke(Action action)
        {
            if (Current.OnMainThread)
            {
                action();
                return;
            }

            using (var ev = new ManualResetEventSlim())
            {
                Current.Queue.Enqueue((action, ev));

                ev.Wait();
            }
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
    }
}
