using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlueDiamond.Desktop
{
    /// <summary>
    /// Class to run a client or server that facilitates network discovery.
    /// </summary>
    public class DiscoveryServer
    {
        /// <summary>
        /// Start the network discovery server
        /// <paramref name="serverName"/>
        /// </summary>
        public static void Start(string serverName)
        {
            Trace.TraceInformation("DisciveryServer.Start");
            if (IsStarted)
                return;

            IsStarted = true;

            Task.Run(() =>
            {
                try
                {
                    ServerTask(serverName);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("DiscoveryServer.ServerTask: Fatal error:\r\n{0}", ex);
                    IsStarted = false;
                }
            });
        }

        /// <summary>
        /// Stop the network discovery server
        /// </summary>
        public static void Stop()
        {
            Trace.TraceInformation("DisciveryServer.Stop");
            IsStarted = false;
        }

        public static bool IsStarted { get; set; }

        public static string ServerResponse { get; set; }

        public static string ServerResponseAddress { get; set; }

        static void ServerTask(string serverName)
        {
            Trace.TraceInformation("DiscoveryServer.ServerTask: starting");

            using (var udpServer = new UdpClient(8888))
            {
                var responseData = Encoding.ASCII.GetBytes(serverName);

                while (IsStarted)
                {
                    try
                    {
                        var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        var clientRequestData = udpServer.Receive(ref clientEndPoint);
                        var requestData = Encoding.ASCII.GetString(clientRequestData);
                        if (requestData == "BlueDiamond")
                        {
                            Trace.TraceInformation("DiscoveryServer.ServerTask: Recived {0} from {1}, sending response", requestData, clientEndPoint.Address.ToString());
                            udpServer.Send(responseData, responseData.Length, clientEndPoint);
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("DiscoveryServer.ServerTask: Error:\r\n{0}", ex);
                    }
                }
            }
            IsStarted = false;
            Trace.TraceInformation("DiscoveryServer.ServerTask: completed");
        }

        /// <summary>
        /// Find a server
        /// </summary>
        /// <param name="request">the request string</param>
        /// <param name="timeout">timeout in milliseconds</param>
        /// <returns></returns>
        public static bool FindServer(string request, int timeout=5000)
        {
            try
            {
                Trace.TraceInformation("DiscoveryServer.FindServer");

                using (var udpClient = new UdpClient())
                {
                    // the data we're going to send - this should be something we can recognize
                    byte[] requestData = Encoding.ASCII.GetBytes(request);

                    // bind to all IP addresses
                    var serverEndPoint = new IPEndPoint(IPAddress.Any, 0);

                    // enable broadcast - this is the magic that sends the packet to every machine
                    udpClient.EnableBroadcast = true;

                    // set the timeout to 
                    udpClient.Client.SendTimeout = timeout;
                    udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);

                    // send the request
                    udpClient.Send(requestData, requestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

                    var serverResponseData = udpClient.Receive(ref serverEndPoint);

                    ServerResponse = Encoding.ASCII.GetString(serverResponseData);
                    ServerResponseAddress = serverEndPoint.Address.ToString();

                    Trace.TraceInformation("Recived {0} from {1}", ServerResponse, serverEndPoint.Address.ToString());

                    udpClient.Close();
                }

                Trace.TraceInformation("DiscoveryServer.FindServer: got response");

                return string.IsNullOrEmpty(ServerResponse);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                    Trace.TraceInformation("DiscoveryServer.FindServer: Could not find server");
                else
                    Trace.TraceError("DiscoveryServer.FindServer: Error:\r\n{0}", ex);
                return false;
            }
            catch (Exception ex)
            {
                Trace.TraceError("DiscoveryServer.FindServer: Error:\r\n{0}", ex);
                return false;
            }
        }
    }
}
