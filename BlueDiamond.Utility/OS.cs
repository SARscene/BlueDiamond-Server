using System;
using System.Net;
using System.Net.Sockets;

namespace BlueDiamond.Utility
{
    public static class OS
    {
        /// <summary>
        /// determine if we're a 64 bit application
        /// </summary>
        public static bool Is64Bit { get { return IntPtr.Size == 8; } }

        /// <summary>
        /// get the IP address of this computer
        /// Note if we have more than one NIC we might have trouble here.
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    localIP = ip.ToString();
            }
            return localIP;
        }

    }
}
