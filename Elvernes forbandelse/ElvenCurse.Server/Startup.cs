using System;
using System.Configuration;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Server.App_Start;
using ElvenCurse.Server.Hubs;
using ElvenCurse.Server.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Owin.Security.AesDataProtectorProvider;

namespace ElvenCurse.Server
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var cookiename = "ElvenCurseAuthcookie";

            var cookie = new CookieAuthenticationOptions
            {
                CookieName = cookiename,
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            };

            NinjectWebCommon.Start();

            var resolver = new NinjectSignalRDependencyResolver(NinjectWebCommon.bootstrapper.Kernel);
            if (NinjectWebCommon.bootstrapper.Kernel == null)
            {
                throw new NullReferenceException("Resolver not found");
            }

            NinjectWebCommon.bootstrapper.Kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<GameHub>().Clients)
                .WhenInjectedInto<IGameEngine>();

            app.Map("/signalr", map =>
            {
                map.UseCookieAuthentication(cookie);
                map.UseAesDataProtectorProvider(ConfigurationManager.AppSettings["cryptokey"]);

                map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
                {
                    Provider = new ApplicationOAuthBearerAuthenticationProvider(cookiename)
                });

                map.UseCors(CorsOptions.AllowAll);
                var config = new HubConfiguration {Resolver = resolver};
                map.RunSignalR(config);
            });
        }
    }
}
