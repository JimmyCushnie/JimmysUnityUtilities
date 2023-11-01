using System;
using System.Timers;

namespace JimmysUnityUtilities
{
    /// <summary>
    /// Enables the use of <see cref="TimeSpan"/> when getting or setting the interval of a <see cref="Timer"/>.
    /// </summary>
    public static class TimerExtensions
    {
        public static TimeSpan GetInterval(this Timer timer)
        {
            return TimeSpan.FromMilliseconds(timer.Interval);
        }

        public static void SetInterval(this Timer timer, TimeSpan interval)
        {
            timer.Interval = interval.TotalMilliseconds;
        }
    }
}