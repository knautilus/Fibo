using System.Web.Http;
using WebApi.StructureMap;

namespace Fibo.Second
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.UseStructureMap<DependencyConfig>();
        }

        protected void Application_End()
        {
            GlobalConfiguration.Configuration.Dispose();
        }
    }
}
