using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using JimmysUnityUtilities.Threading;

namespace JimmysUnityUtilities.Pings
{
    /// <summary>
    /// A wrapper for <see cref="Ping"/> with a more convenient API, particularly for Unity applications.
    /// Can send several different pings and average their round-trip time to get network latency.
    /// </summary>
    public class NetworkPinger
    {
        private IPAddress TargetAddress;
        private string HostNameOrAddress;

        public NetworkPinger(IPAddress address)
        {
            TargetAddress = address;
        }
        public NetworkPinger(string hostNameOrAddress)
        {
            HostNameOrAddress = hostNameOrAddress;
        }


        LockedList<Ping> IndividualPings = new LockedList<Ping>();
        LockedList<long> PingResponseTimes = new LockedList<long>();

        Action<PingSuccess> PingSuccessCallback;
        Action<PingFailure> PingFailureCallback;

        public void PingDestination(Action<PingSuccess> onPingSuccessCallback, Action<PingFailure> onPingFailureCallback, int numberOfSeparatePings = 10, int timeOutMilliseconds = 5000)
        {
            PingSuccessCallback = onPingSuccessCallback;
            PingFailureCallback = onPingFailureCallback;

            CancelPendingPings();


            Task.Run(() =>
            {
                // We run this on another thread because if an invalid hostname is provided it takes literally five entire seconds
                // for .net to figure that out. Fuck you .net
                if (TargetAddress == null)
                {
                    try
                    {
                        TargetAddress = NetworkUtilities.ParseServerIP(HostNameOrAddress);
                    }
                    catch
                    {
                        TriggerPingFailure(PingFailureReason.AddressNotFound);
                        return;
                    }
                }

                if (TargetAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    // Sorry about this... Mono's Ping() doesn't support IPv6. I tried to add support (see the 'fixedping' branch) but
                    // Unity's Mono fork has a bug where you can't create IPv6 ICMP sockets. This bug is *not* present on upstream
                    // Mono. I've reported the bug to Unity, as soon as they fix it I should be able to add IPv6 suport to this class.
                    TriggerPingFailure(PingFailureReason.IPv6Unsupported);
                    return;
                }

                for (int i = 0; i <= numberOfSeparatePings; i++)
                {
                    var ping = new Ping();
                    IndividualPings.Add(ping);

                    ping.PingCompleted += OnPingCompleted;
                    ping.SendAsync(TargetAddress, timeOutMilliseconds, null);
                }
            });
        }


        public void CancelPendingPings()
        {
            lock (IndividualPings.__InternalListLock)
            {
                foreach (var ping in IndividualPings)
                {
                    try
                    {
                        ping.SendAsyncCancel();
                    }
                    catch (InvalidOperationException) { } // No active async requests to cancel
                }
            }

            IndividualPings.Clear();
            PingResponseTimes.Clear();
        }

        private void OnPingCompleted(object _, PingCompletedEventArgs args)
        {
            if (args.Cancelled)
                return;

            if (args.Error != null)
            {
                TriggerPingFailure(IPStatus.Unknown);
                return;
            }

            PingReply reply = args.Reply;
            if (reply.Status != IPStatus.Success)
            {
                TriggerPingFailure(reply.Status);
                return;
            }


            PingResponseTimes.Add(reply.RoundtripTime);
            
            if (PingResponseTimes.Count >= IndividualPings.Count)
            {
                CancelPendingPings();

                long averageTime;

                // Must use the lock here as GetMean() iterates over the collection
                lock (PingResponseTimes.__InternalListLock)
                    averageTime = PingResponseTimes.GetMean();

                Dispatcher.InvokeAsync(() =>
                {
                    // Invoke on the main thread
                    PingSuccessCallback.Invoke(new PingSuccess() { AverageRoundTripTimeMilliseconds = averageTime });
                    ClearCallbacks();
                });
            }
        }


        private void TriggerPingFailure(IPStatus status)
            => TriggerPingFailure((PingFailureReason)status);

        private void TriggerPingFailure(PingFailureReason failure)
        {
            CancelPendingPings();
            Dispatcher.InvokeAsync(() =>
            {
                // Invoke on the main thread
                PingFailureCallback?.Invoke(new PingFailure() { FailureReason = failure });
                ClearCallbacks();
            });
        }

        private void ClearCallbacks()
        {
            PingSuccessCallback = null;
            PingFailureCallback = null;
        }
    }
}