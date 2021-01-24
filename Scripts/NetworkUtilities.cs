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


        public static bool TryParseServerIpAndPort(string address, int defaultPort, out (IPAddress ip, int port) result)
        {
            try
            {
                result = ParseServerIpAndPort(address, defaultPort);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Convert an IP address and port in the form ip:port
        /// </summary>
        /// <param name="defaultPort">The port that will be returned if the provided string does not contain a port.</param>
        public static (IPAddress ip, int port) ParseServerIpAndPort(string address, int defaultPort)
        {
            string ip = address;
            int port = defaultPort;

            if (address.Contains(":"))
            {
                var parts = address.Split(':');
                ip = parts[0];
                port = int.Parse(parts[1]);
            }

            return (ParseServerIP(ip), port);
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