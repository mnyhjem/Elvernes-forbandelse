using ElvenCurse.Core.Interfaces;
using ElvenCurse.Website.App_Start;
using ElvenCurse.Website.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
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

            
            var resolver = new NinjectSignalRDependencyResolver(NinjectWebCommon.bootstrapper.Kernel);
            NinjectWebCommon.bootstrapper.Kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                        resolver.Resolve<IConnectionManager>().GetHubContext<GameHub>().Clients)
                .WhenInjectedInto<IGameEngine>();

            var config = new HubConfiguration();
            config.Resolver = resolver;
            app.MapSignalR(config);
        }
    }
}
