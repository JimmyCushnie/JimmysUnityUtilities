using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.Collections.Generic;

namespace JimmysUnityUtilities.Networking
{
    public static class NetworkUtilities
    {
        public static int GetAvailablePort()
        {
            int port;

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var localEndPoint = new IPEndPoint(IPAddress.Any, 0);
                socket.Bind(localEndPoint);
                port = ((IPEndPoint)socket.LocalEndPoint).Port;
                socket.Close();
            }

            return port;
        }


        /// <summary>
        /// Gets all addresses of this device on all the networks this device is part of. Other devices on this device's networks can reach it by using these addresses.
        /// </summary>
        public static IEnumerable<IPAddress> GetAllLocalNetworkAddressesOfThisDevice()
        {
            // You might be tempted to refactor this by using IPGlobalProperties.GetIPGlobalProperties().GetUnicastAddresses()
            // instead of iterating through all the network interfaces. DON'T, however; in some .NET implementations (mono)
            // that function isn't supported and will throw an exception.

            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var unicast in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (unicast.Address.IsLocalNetworkAddress())
                            yield return unicast.Address;
                    }
                }
            }
        }
    }
}