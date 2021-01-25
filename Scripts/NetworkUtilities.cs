using System;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Globalization;

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
        /// Like <see cref="ParseServerEndpoint(string, int)"/> but with graceful failure handling.
        /// </summary>
        public static bool TryParseServerEndpoint(string source, int defaultPort, out IPEndPoint endpoint)
        {
            try
            {
                endpoint = ParseServerEndpoint(source, defaultPort);
                return true;
            }
            catch
            {
                endpoint = default;
                return false;
            }
        }

        /// <summary>
        /// Parse a string that refers to an IP endpoint in the format ip:port or [ip]:port (for IPv6). This is the kind of string a user might enter to connect to the server.
        /// </summary>
        /// <param name="defaultPort">The port that will be returned if <paramref name="address"/> does not contain a port.</param>
        public static IPEndPoint ParseServerEndpoint(string source, int defaultPort)
        {
            int addressLength = source.Length;
            int lastColonIndex = source.LastIndexOf(':');

            if (lastColonIndex > 0)
            {
                if (source[lastColonIndex - 1] == ']')
                {
                    // This is an IPv6 address with a port
                    addressLength = lastColonIndex;
                }
                else if (source.IndexOf(':') == lastColonIndex)
                {
                    // This is an IPv4 address with a port
                    addressLength = lastColonIndex;
                }
            }

            string addressString = source.Substring(0, addressLength);
            IPAddress address = ParseServerIP(addressString);

            int port = defaultPort;
            if (source.Length > addressLength)
                port = int.Parse(source.Substring(addressLength + 1), NumberStyles.None, CultureInfo.InvariantCulture);

            return new IPEndPoint(address, port);
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
        /// Convert a string to an ip address. Handles standard format IPv4 and IPv6 addresses, hostnames, and the "localhost" case.
        /// </summary>
        /// <remarks>
        /// If a string is entered that is not a valid IP, this method will take 5 entire seconds to fail the DNS 
        /// lookup (dotnet doesn't let you change this). Therefore, this method should be run asynchronously when
        /// possible.
        /// </remarks>
        public static IPAddress ParseServerIP(string ip)
        {
            if (ip == "localhost")
                return IPAddress.Loopback; // Maybe this should be IPAddress.IPv6Loopback ?

            if (IPAddress.TryParse(ip, out var ipAddress))
                return ipAddress;

            try
            {
                var hostEntry = Dns.GetHostEntry(ip); // This will time out after 5 seconds (not configurable)
                if (hostEntry.AddressList.Length > 0)
                    return hostEntry.AddressList[0]; // I'm not sure what should happen if a host has multiple addresses. This works for now ¯\_(ツ)_/¯
            }
            catch (SocketException)
            {
            }

            throw new Exception("Failed to resolve host");
        }
    }
}