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


        // Todo: support ipv6 with the below methods

        /// <summary>
        /// Like <see cref="ParseServerEndpoint(string, int)"/> but with graceful failure handling.
        /// </summary>
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
        /// Parse a string that refers to an IP endpoint in the format ip:port. This is the kind of string a user might enter to connect to the server.
        /// </summary>
        /// <param name="defaultPort">The port that will be returned if <paramref name="address"/> does not contain a port.</param>
        public static IPEndPoint ParseServerEndpoint(string address, int defaultPort)
        {
            string ip = address;
            int port = defaultPort;

            if (address.Contains(":"))
            {
                var parts = address.Split(':');

                if (parts.Length != 2)
                    throw new FormatException($"Cannot parse {address} as {nameof(IPEndPoint)}");

                ip = parts[0];
                port = int.Parse(parts[1]);
            }

            return new IPEndPoint(ParseServerIP(ip), port);
        }

        /// <summary>
        /// Like <see cref="ParseServerIP(string)"/> but with graceful failure handling.
        /// </summary>
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
        /// <remarks>
        /// If a string is entered that is not a valid IP, this method will take 5 entire seconds to fail the DNS 
        /// lookup (dotnet doesn't let you change this). Therefore, this method should be run asynchronously when
        /// possible.
        /// </remarks>
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
                var hostEntry = Dns.GetHostEntry(ip); // This will time out after 5 seconds (not configurable)
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