using System.Web.Http;
using OnlineAuction.BLL.Infrastructure;
using OnlineAuction.BLL.Infrastructure.AutoMapper;

namespace OnlineAuction.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperServicesConfiguration.Configure();
        }
    }
}
