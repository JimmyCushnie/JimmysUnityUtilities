using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    /// <summary>
    /// The built-in WaitForSeconds is not precise if you need to use many of them consecutively.
    /// Each WaitForSeconds can be off by up to one frame, and this errors can add up.
    /// </summary>
    /// <remarks>
    /// Create one PreciseSecondsCounter for each routine you use.
    /// </remarks>
    public class WaitForSecondsPrecise : CustomYieldInstruction
    {
        public override bool keepWaiting
            => !Counter.TotalWaitTimeMet;

        private PreciseSecondsCounter Counter;
        public WaitForSecondsPrecise(PreciseSecondsCounter counter, float seconds)
        {
            this.Counter = counter;
            counter.AddWait(seconds);
        }
    }

    public class PreciseSecondsCounter
    {
        private readonly float TimeOnCreation;
        public PreciseSecondsCounter() 
        {
            TimeOnCreation = Time.time;
        }

        private float totalWaitTime = 0;
        internal void AddWait(float seconds)
            => totalWaitTime += seconds;

        internal bool TotalWaitTimeMet
            => Time.time >= TimeOnCreation + totalWaitTime;
    }
}