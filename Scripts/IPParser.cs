using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace JimmysUnityUtilities
{
    public static class IPParser
    {
        /// <summary>
        /// Like <see cref="ParseEndpoint(string, int)"/> but with graceful failure handling.
        /// </summary>
        public static bool TryParseEndpoint(string source, int defaultPort, out IPEndPoint endpoint, AddressFamily? preferredAddressType = null)
        {
            try
            {
                endpoint = ParseEndpoint(source, defaultPort, preferredAddressType);
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
        public static IPEndPoint ParseEndpoint(string source, int defaultPort, AddressFamily? preferredAddressType = null)
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
            IPAddress address = ParseAddress(addressString, preferredAddressType);

            int port = defaultPort;
            if (source.Length > addressLength)
                port = int.Parse(source.Substring(addressLength + 1), NumberStyles.None, CultureInfo.InvariantCulture);

            return new IPEndPoint(address, port);
        }

        /// <summary>
        /// Like <see cref="ParseAddress(string)"/> but with graceful failure handling.
        /// </summary>
        public static bool TryParseAddress(string ip, out IPAddress ipAddress, AddressFamily? preferredAddressType = null)
        {
            try
            {
                ipAddress = ParseAddress(ip, preferredAddressType);
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
        public static IPAddress ParseAddress(string ip, AddressFamily? preferredAddressType = null)
        {
            if (ip == "localhost")
            {
                switch (LocalHostInterpretation)
                {
                    case LocalHostInterpretation.IPv4Loopback:
                        return IPAddress.Loopback;

                    case LocalHostInterpretation.IPv6Loopback:
                        return IPAddress.IPv6Loopback;

                    default:
                        throw new NotSupportedException($"{nameof(LocalHostInterpretation)} had an unexpected value of {LocalHostInterpretation}");
                }
            }

            if (IPAddress.TryParse(ip, out var ipAddress))
                return ipAddress;

            try
            {
                var hostEntry = Dns.GetHostEntry(ip); // This will time out after 5 seconds (not configurable)
                var validResults = hostEntry.AddressList
                    .Where(a => a.AddressFamily == AddressFamily.InterNetwork || a.AddressFamily == AddressFamily.InterNetworkV6)
                    .OrderByDescending(address => address.AddressFamily == preferredAddressType ? 1 : 0).ToArray();
                if (validResults.Length > 0)
                    return validResults[0];
            }
            catch (SocketException) { }

            throw new Exception("Failed to resolve host");
        }


        /// <summary>
        /// Determines how "localhost" should be used when parsing IP addresses and endpoints
        /// </summary>
        public static LocalHostInterpretation LocalHostInterpretation { get; set; } = LocalHostInterpretation.IPv4Loopback;
    }

    public enum LocalHostInterpretation
    {
        IPv4Loopback,
        IPv6Loopback,
    }
}
