using System.Net;
using System.Net.Sockets;

namespace JimmysUnityUtilities
{
    public static class IPAddressExtensions
    {
        /// <summary>
        /// Returns true if the address falls within a block used for devices to communicate with each other within a local network.
        /// For IPv4 addresses this is:
        /// [10.0.0.0 - 10.255.255.255],
        /// [169.254.0.0 - 169.254.255.255],
        /// [172.16.0.0 - 172.31.255.255],
        /// [192.168.0.0 - 192.168.255.255].
        /// For IPv6 addresses this is [ff00:: - ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff].
        /// </summary>
        public static bool IsLocalNetworkAddress(this IPAddress address)
        {
            if (address.IsIPv4MappedToIPv6)
                address = address.MapToIPv4();

            if (address.AddressFamily == AddressFamily.InterNetworkV6)
            {
                return address.IsIPv6LinkLocal;
            }
            else if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                byte[] ip = address.GetAddressBytes();
                switch (ip[0])
                {
                    case 10:
                        return true;
                    case 172:
                        return ip[1] >= 16 && ip[1] < 32;
                    case 192:
                        return ip[1] == 168;

                    // IPv4 Link-local address, as specified in RFC3927
                    case 169:
                        return ip[1] == 254;

                    default:
                        return false;
                }
            }
            else
            {
                // Invalid address family
                return false;
            }
        }
    }
}