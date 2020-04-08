// Based on https://wiki.unity3d.com/index.php?title=CustomFixedUpdate&oldid=18294 by Bunny83

using UnityEngine;
using System;

namespace JimmysUnityUtilities
{
    /// <summary>
    /// Like Unity's FixedUpdate, but you can have many of them, and they can all have different tickrates independant from the physics simulation.
    /// </summary>
    public class CustomFixedUpdate
    {
        /// <summary>
        /// FixedUpdateCallback is called every this number of seconds.
        /// </summary>
        public float FixedTimeStep { get; set; }

        /// <summary>
        /// FixedUpdateCallback is called this number of times per second.
        /// </summary>
        public float UpdatesPerSecond
        {
            get => 1.0f / FixedTimeStep;
            set => FixedTimeStep = 1.0f / value;
        }

        /// <summary>
        /// If this value is non-zero, the tickrate can slow down to account for lag.
        /// </summary>
        public float MaxAllowedTimeStep { get; set; }


        private Action FixedUpdateCallback;

        /// <param name="fixedTimeStep"> fixedUpdateCallback is called every this number of seconds </param>
        /// <param name="fixedUpdateCallback"> the callback of the CustomFixedUpdate </param>
        /// <param name="maxAllowedTimestep"> maximum allowed timestep. Set to something other than zero (0.5f is recommended) to prevent a death spiral if the game starts lagging. </param>
        public CustomFixedUpdate(float fixedTimeStep, Action fixedUpdateCallback, float maxAllowedTimestep = 0)
        {
            if (fixedUpdateCallback == null)
                throw new ArgumentException("CustomFixedUpdate needs a valid callback");

            if (fixedTimeStep <= 0f)
                throw new ArgumentException("TimeStep needs to be greater than 0");


            FixedTimeStep = fixedTimeStep;
            FixedUpdateCallback = fixedUpdateCallback;
            MaxAllowedTimeStep = maxAllowedTimestep;
        }

        /// <param name="updatesPerSecond"> fixedUpdateCallback is called this number of times per second </param>
        /// <param name="fixedUpdateCallback"> the callback of the CustomFixedUpdate </param>
        /// <param name="maxAllowedTimestep"> maximum allowed timestep. Set to something other than zero (0.5f is recommended) to prevent a death spiral if the game starts lagging. </param>
        public CustomFixedUpdate(Action fixedUpdateCallback, float updatesPerSecond, float maxAllowedTimestep)
        {
            if (fixedUpdateCallback == null)
                throw new ArgumentException("CustomFixedUpdate needs a valid callback");

            UpdatesPerSecond = updatesPerSecond;
            FixedUpdateCallback = fixedUpdateCallback;
            MaxAllowedTimeStep = maxAllowedTimestep;
        }


        /// <summary>
        /// Call this method whenever you want the callback to trigger. It will trigger call FixedUpdateCallback the correct number of times since the previous frame.
        /// </summary>
        public void Update()
        {
            Update(Time.deltaTime);
        }

        /// <summary>
        /// Like the other Update(), but you can specify a custom time since the last update.
        /// </summary>
        private float Timer = 0;
        public void Update(float aDeltaTime)
        {
            Timer -= aDeltaTime;
            if (MaxAllowedTimeStep > 0)
            {
                float timeout = Time.realtimeSinceStartup + MaxAllowedTimeStep;
                while (Timer < 0f && Time.realtimeSinceStartup < timeout)
                {
                    FixedUpdateCallback();
                    Timer += FixedTimeStep;
                }
            }
            else
            {
                while (Timer < 0f)
                {
                    FixedUpdateCallback();
                    Timer += FixedTimeStep;
                }
            }
        }
    }
}