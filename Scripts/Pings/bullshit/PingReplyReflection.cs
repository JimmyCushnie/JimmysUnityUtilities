using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;

namespace JimmysUnityUtilities.Pings.Bullshit
{
    internal static class PingReplyReflection
    {
        private static readonly ConstructorInfo ConstructorInfo;
        private static readonly ConstructorInfo ConstructorInfo_ipStatus;
        private static readonly ConstructorInfo ConstructorInfo_data_dataLength_address_time;
        private static readonly ConstructorInfo ConstructorInfo_address_buffer_options_roundtripTime_status;
        // This list is incomplete -- there are two constructors missing because their parameters are internal types, but thankfully Ping doesn't need them.

        static PingReplyReflection()
        {
            var pingReplyType = typeof(PingReply);

            ConstructorInfo                                             = pingReplyType.GetConstructor(new Type[] { });
            ConstructorInfo_ipStatus                                    = pingReplyType.GetConstructor(new Type[] { typeof(IPStatus) });
            ConstructorInfo_data_dataLength_address_time                = pingReplyType.GetConstructor(new Type[] { typeof(byte[]), typeof(int), typeof(IPAddress), typeof(int) });
            ConstructorInfo_address_buffer_options_roundtripTime_status = pingReplyType.GetConstructor(new Type[] { typeof(IPAddress), typeof(byte[]), typeof(PingOptions), typeof(long), typeof(IPStatus) });
        }


        public static PingReply Constructor()
            => (PingReply)ConstructorInfo.Invoke(new object[] { });

        public static PingReply Constructor(IPStatus ipStatus)
            => (PingReply)ConstructorInfo_ipStatus.Invoke(new object[] { ipStatus });

        public static PingReply Constructor(byte[] data, int dataLength, IPAddress address, int time)
            => (PingReply)ConstructorInfo_data_dataLength_address_time.Invoke(new object[] { data, dataLength, address, time });

        public static PingReply Constructor(IPAddress address, byte[] buffer, PingOptions options, long roundtripTime, IPStatus status)
            => (PingReply)ConstructorInfo_address_buffer_options_roundtripTime_status.Invoke(new object[] { address, buffer, options, roundtripTime, status });
    }
}