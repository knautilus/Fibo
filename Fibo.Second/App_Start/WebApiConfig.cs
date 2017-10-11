using System.Web.Http;
using Fibo.Utils;

namespace Fibo.Second
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Converters.Add(new BigIntegerConverter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
