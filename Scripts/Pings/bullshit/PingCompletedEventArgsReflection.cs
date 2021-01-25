using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;

namespace JimmysUnityUtilities.Pings.Bullshit
{
    internal static class PingCompletedEventArgsReflection
    {
        private static readonly ConstructorInfo ConstructorInfo;

        static PingCompletedEventArgsReflection()
        {
            var pingCompletedEventArgsType = typeof(PingCompletedEventArgs);
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;

            ConstructorInfo = pingCompletedEventArgsType.GetConstructor(flags, null, new Type[] { typeof(Exception), typeof(bool), typeof(object), typeof(PingReply) }, null);
        }


        public static PingCompletedEventArgs Constructor(Exception ex, bool cancelled, object userState, PingReply reply)
            => (PingCompletedEventArgs)ConstructorInfo.Invoke(new object[] { ex, cancelled, userState, reply});
    }
}