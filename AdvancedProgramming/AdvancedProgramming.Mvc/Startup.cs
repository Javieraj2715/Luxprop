using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartup(typeof(AdvancedProgramming.Mvc.Startup))]

namespace AdvancedProgramming.Mvc
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR(new HubConfiguration { EnableDetailedErrors = true });
        }
    }
}
