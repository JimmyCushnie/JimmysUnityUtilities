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


        public static bool TryParseServerEndpoint(string address, int defaultPort, out IPEndPoint endpoint)
        {
            try
            {
                endpoint = ParseServerEndpoint(address, defaultPort);
                return true;
            }
            catch
            {
                endpoint = default;
                return false;
            }
        }

        /// <summary>
        /// Convert an IP address and port in the form ip:port
        /// </summary>
        /// <param name="defaultPort">The port that will be returned if the provided string does not contain a port.</param>
        public static IPEndPoint ParseServerEndpoint(string address, int defaultPort)
        {
            string ip = address;
            int port = defaultPort;

            if (address.Contains(":"))
            {
                var parts = address.Split(':');
                ip = parts[0];
                port = int.Parse(parts[1]);
            }

            return new IPEndPoint(ParseServerIP(ip), port);
        }

        public static bool TryParseServerIP(string ip, out IPAddress ipAddress)
        {
            try
            {
                ipAddress = ParseServerIP(ip);
                return true;
            }
            catch
            {
                ipAddress = default;
                return false;
            }
        }

        /// <summary>
        /// Convert a string to an ip address. Handles numbered IPs, hostname ips, and the "localhost" case.
        /// </summary>
        public static IPAddress ParseServerIP(string ip)
        {
            if (ip.Contains(":")) // If user passes string with port, handle that case and return the IP
                return ParseServerEndpoint(ip, 0).Address;

            if (ip == "localhost")
                return IPAddress.Loopback;
            
            if (IPAddress.TryParse(ip, out var ipAddress))
                return ipAddress;

            try
            {
                var hostEntry = Dns.GetHostEntry(ip); // This will time out after 5 seconds (not configurable). Therefore, the hostname could be invalid, this method should be called async.
                if (hostEntry.AddressList.Length > 0)
                    return hostEntry.AddressList[0];
            }
            catch (SocketException)
            {
            }

            throw new Exception("Failed to resolve host");
        }
    }
}