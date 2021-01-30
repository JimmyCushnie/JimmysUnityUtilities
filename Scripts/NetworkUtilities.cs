using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.Collections.Generic;

namespace JimmysUnityUtilities
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
            // You could also get all unicast addresses by iterating through all the network interfaces.

            var allAvailableUnicasts = IPGlobalProperties.GetIPGlobalProperties().GetUnicastAddresses();
            foreach (var unicast in allAvailableUnicasts)
            {
                if (unicast.Address.IsLocalNetworkAddress())
                    yield return unicast.Address;
            }
        }
    }
}