using System;

namespace JimmysUnityUtilities
{
    public static class UnixTimestamps
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long DateTimeToUnixTimestamp(DateTime dateTime) 
            => (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalSeconds;

        public static DateTime UnixTimestampToDateTime(long unixTimestamp) 
            => UnixEpoch.AddSeconds(unixTimestamp);
    }
}
