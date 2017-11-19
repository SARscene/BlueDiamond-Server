using Microsoft.Owin;
using Owin;
using System.Diagnostics;

[assembly: OwinStartupAttribute(typeof(BlueDiamond.Startup))]
namespace BlueDiamond
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IsIISExpress = string.Compare(Process.GetCurrentProcess().ProcessName, "iisexpress") == 0;

            if (IsIISExpress)
                Trace.TraceInformation("Running on IIS Express");
            else
                Trace.TraceInformation("Running on IIS");

            ConfigureAuth(app);
            app.MapSignalR();
        }

        public bool IsIISExpress { get; set; }
    }


}
