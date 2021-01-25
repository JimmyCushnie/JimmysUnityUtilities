using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;

namespace JimmysUnityUtilities.Pings.Bullshit
{
    internal static class SocketReflection
    {
        // Socket has like 30 overloads for RecieveFrom, and of course Ping uses the ONE overload that isn't public.
        // This was a bitch to figureo ut with the ref type and the out parameter.

        private static readonly MethodInfo MethodInfo;

        static SocketReflection()
        {
            var socketType = typeof(Socket);
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;

            MethodInfo = socketType.GetMethod("ReceiveFrom", flags, null, new Type[] { typeof(byte[]), typeof(int), typeof(int), typeof(SocketFlags), typeof(EndPoint).MakeByRefType(), typeof(SocketError).MakeByRefType() }, null);
        }


        public static int ReceiveFrom(this Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, out SocketError errorCode)
        {
            var parameters = new object[] { buffer, offset, size, socketFlags, remoteEP, null };
            int value = (int)MethodInfo.Invoke(socket, parameters);

            errorCode = (SocketError)parameters[5];
            return value;
        }
    }
}