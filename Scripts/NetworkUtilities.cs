using System;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

namespace JimmysUnityUtilities
{
    public static class NetworkUtilities
    {
        public static int GetAvailablePort()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var localEndPoint = new IPEndPoint(IPAddress.Any, 0);
                socket.Bind(localEndPoint);
                localEndPoint = (IPEndPoint)socket.LocalEndPoint;
                return localEndPoint.Port;
            }
        }
    }
}