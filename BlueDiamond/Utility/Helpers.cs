using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using BlueDiamond.Hubs;

namespace BlueDiamond.Utility
{
    public static class Helpers
    {
        /// <summary>
        /// Get the version of this service
        /// </summary>
        /// <returns></returns>
        internal static string GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        /// <summary>
        /// Send a message via signalR
        /// </summary>
        /// <param name="item"></param>
        internal static void SignalSignIn(Guid incidentID, string lastName, string firstName, DateTime loginTime)
        {
            try
            {
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<EventHub>();
                context.Clients.Group(incidentID.ToString())
                    .login(lastName,firstName,loginTime.ToLocalTime());
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error:\r\n{0}", ex);
                //item.Status += string.Format("\r\nMesasge not sent: error was {0}", ex);
            }
        }

        /// <summary>
        /// Converts the value of the current DateTime object to the date and time specified by an offset value.
        /// </summary>
        /// <param name="dt" />DateTime value.</param>
        /// <param name="offset" />The offset to convert the DateTime value to.</param>
        /// <returns>DateTime value that is local to an offset.</returns>
        public static DateTime ToOffset(this DateTime dt, TimeSpan offset)
        {
            return dt.ToUniversalTime().Add(offset);
        }

        public static DateTime ToLocal(this DateTime dt, TimeZoneInfo info)
        {
            return (info != null) ? TimeZoneInfo.ConvertTimeFromUtc(dt, info) : dt.ToLocalTime();
        }

        public static DateTime? ToLocal(this DateTime? dt, TimeZoneInfo info)
        {
            if (dt == null)
                return null;
            return (info != null) ? TimeZoneInfo.ConvertTimeFromUtc(dt.Value, info) : dt.Value.ToLocalTime();
        }

        public static DateTime ToUtc(this DateTime dt, TimeZoneInfo info)
        {
            return (info != null) ? TimeZoneInfo.ConvertTimeToUtc(dt, info) : dt;
        }
        
        public static DateTime? ToUtc(this DateTime? dt, TimeZoneInfo info)
        {
            if (dt == null)
                return null;
            return (info != null) ? TimeZoneInfo.ConvertTimeToUtc(dt.Value, info) : dt;
        }
    }
}