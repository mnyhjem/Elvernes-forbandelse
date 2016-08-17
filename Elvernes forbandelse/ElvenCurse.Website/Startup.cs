using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ElvenCurse.Website.Startup))]
namespace ElvenCurse.Website
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
