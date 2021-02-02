using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace JimmysUnityUtilities.Networking.Broadcasting
{
    /// <summary>
    /// Internal class used by <see cref="LocalNetworkCommunicator"/>
    /// </summary>
    internal class BoundBroadcaster : IDisposable
    {
        public static bool TryCreate(IPAddress localAddress, int port, out BoundBroadcaster broadcaster)
        {
            if (localAddress.AddressFamily != AddressFamily.InterNetwork)
            {
                broadcaster = null;
                return false;
            }

            try
            {
                broadcaster = new BoundBroadcaster(localAddress, port);
                return true;
            }
            catch (SocketException)
            {
                broadcaster = null;
                return false;
            }
        }

        public int Port { get; }
        private UdpClient Client { get; }

        private BoundBroadcaster(IPAddress localAddress, int port)
        {
            // I have spent so much god damn time trying to get this piece of shit to work with IPv6 using ff02::1. Sorry, I just couldn't figure it out.
            // It should be fine for the next couple decades, as essentially every local network in the world supports IPv4 and that's not liable to
            // change any time soon.
            // TODO: figure out IPv6!
            if (localAddress.AddressFamily == AddressFamily.InterNetworkV6)
                throw new NotSupportedException($"{nameof(BoundBroadcaster)} does not support IPv6. The local address must be IPv4.");


            Port = port;

            Client = new UdpClient();

            var socket = Client.Client;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.ExclusiveAddressUse = false;

            socket.Bind(new IPEndPoint(localAddress, Port));
            BroadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, port);
        }

        private readonly IPEndPoint BroadcastEndPoint;
        public void Send(byte[] data)
        {
            Client.Send(data, data.Length, BroadcastEndPoint);
        }

        public bool IsReceivingData { get; private set; } = false;
        public void StartRecievingData(Action<UdpReceiveResult> onDataRecieved)
        {
            if (IsReceivingData)
                throw new InvalidOperationException("A data receiving thread is already running");

            Task.Run(async () =>
            {
                while (!this.IsDisposed)
                {
                    var receivedResults = await Client.ReceiveAsync();
                    onDataRecieved.Invoke(receivedResults);
                }
            });

            IsReceivingData = true;
        }

        private bool IsDisposed = false;
        public void Dispose()
        {
            Client.Dispose();
            IsDisposed = true;
        }
    }
}
