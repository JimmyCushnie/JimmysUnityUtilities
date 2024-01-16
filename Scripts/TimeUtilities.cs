using System;

namespace JimmysUnityUtilities
{
    public static class TimeUtilities
    {
        /// <summary>
        /// Gets a string representing the offset of <see cref="time"/> from UTC (i.e. "UTC-03:00", "UTC+05:30", etc.)
        /// </summary>
        public static string GetUtcOffsetString(DateTimeOffset time)
        {
            string utcOffset = time.Offset.ToString("hh\\:mm");
            return (time.Offset >= TimeSpan.Zero ? "UTC+" : "UTC-") + utcOffset;
        }
    }
}
