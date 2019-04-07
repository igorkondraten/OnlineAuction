using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OnlineAuction.API.Startup))]

namespace OnlineAuction.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}
