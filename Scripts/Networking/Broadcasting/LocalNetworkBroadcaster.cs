using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace JimmysUnityUtilities.Networking.Broadcasting
{
    /// <summary>
    /// Broadcast data, and listen for broadcasted data, with all other <see cref="LocalNetworkCommunicator"/>s on a local network 
    /// that this device is connected to.
    /// </summary>
    /// <remarks>
    /// This will receive messages it sends.
    /// If the device has multiple network interfaces that allow broadcasting, there can be duplicate messages. If that matters
    /// to your use case, make sure you check for duplicates.
    /// Transmission is not 100% reliable. As UDP is used for transmission, packets occasionally get dropped.
    /// </remarks>
    public class LocalNetworkCommunicator : IDisposable
    {
        private readonly List<BoundBroadcaster> AllBroadcasters = new List<BoundBroadcaster>();

        public LocalNetworkCommunicator(int port)
        {
            foreach (var localAddress in NetworkUtilities.GetAllLocalNetworkAddressesOfThisDevice())
            {
                if (BoundBroadcaster.TryCreate(localAddress, port, out var broadcaster))
                    AllBroadcasters.Add(broadcaster);
            }
        }

        private event Action<UdpReceiveResult> _OnDataReceived;
        private readonly object __OnDataReceivedLock = new object();
        public event Action<UdpReceiveResult> OnDataReceived
        {
            add
            {
                lock (__OnDataReceivedLock)
                {
                    _OnDataReceived += value;

                    foreach (var broadcaster in AllBroadcasters)
                    {
                        if (!broadcaster.IsReceivingData)
                            broadcaster.StartRecievingData(OnBroadcastDataReceived);
                    }
                }
            }
            remove
            {
                lock (__OnDataReceivedLock)
                {
                    _OnDataReceived -= value;
                }
            }
        }




        private void OnBroadcastDataReceived(UdpReceiveResult result)
        {
            _OnDataReceived?.Invoke(result);
        }

        public void Broadcast(byte[] data)
        {
            foreach (var broadcaster in AllBroadcasters)
                broadcaster.Send(data);
        }

        public void Dispose()
        {
            foreach (var broadcaster in AllBroadcasters)
                broadcaster.Dispose();
        }
    }
}
