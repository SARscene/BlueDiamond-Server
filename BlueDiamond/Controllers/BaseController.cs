using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using NodaTime.TimeZones;
using BlueDiamond.Utility;

namespace BlueDiamond.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ViewBag.Version = Helpers.GetVersion();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext == null)
                return;
            
            #region get the time zone offset
            
            ViewBag.TimeZoneOffset = TimeSpan.FromMinutes(0); // Default offset (Utc) if cookie is missing.

            var timeZoneCookie = requestContext.HttpContext.Request.Cookies["_timeZoneOffset"];
            if (timeZoneCookie != null)
            {
                double offsetMinutes = 0;
                if (double.TryParse(timeZoneCookie.Value, out offsetMinutes))
                {
                    // Store in ViewBag. You can use Session, TempData, or anything else.
                    ViewBag.TimeZoneOffset = TimeSpan.FromMinutes(offsetMinutes);
                }
            }
            #endregion

            #region get the time zone

            timeZoneCookie = requestContext.HttpContext.Request.Cookies["_timeZoneName"];
            
            if (timeZoneCookie != null)
            {
                TimeZoneInfo info = GetTimeZoneInfoForTzdbId(timeZoneCookie.Value);
                if (info != null)
                    ViewBag.TimeZoneInfo = info;
            }
            
            #endregion

        }
        TimeZoneInfo GetTimeZoneInfoForTzdbId(string tzdbId)
        {
            var mappings = TzdbDateTimeZoneSource.Default.WindowsMapping.MapZones;
            var map = mappings.FirstOrDefault(x =>
                x.TzdbIds.Any(z => z.Equals(tzdbId, StringComparison.OrdinalIgnoreCase)));
            return map == null ? null : TimeZoneInfo.FindSystemTimeZoneById(map.WindowsId);
        }

    }
}