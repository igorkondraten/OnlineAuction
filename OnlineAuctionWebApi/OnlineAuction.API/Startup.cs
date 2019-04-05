using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OnlineAuction.API.Startup))]

namespace OnlineAuction.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
