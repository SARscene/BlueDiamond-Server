using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlueDiamond.Startup))]
namespace BlueDiamond
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }

    }


}
