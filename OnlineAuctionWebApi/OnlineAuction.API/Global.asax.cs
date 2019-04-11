using System.Web.Http;
using Newtonsoft.Json.Serialization;
using OnlineAuction.BLL.Infrastructure;
using OnlineAuction.BLL.Infrastructure.AutoMapper;

namespace OnlineAuction.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            AutoMapperServicesConfiguration.Configure();
        }
    }
}
