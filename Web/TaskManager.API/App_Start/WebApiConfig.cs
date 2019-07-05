using System.Web.Http;
using System.Web.Http.Cors;

namespace TaskManager.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var enableCorsAttribute = new EnableCorsAttribute(
                origins: "*",
                headers: "Origin, Content-Type, Accept, Authorization",
                methods: "GET, POST, OPTIONS"
                );
            config.EnableCors(enableCorsAttribute);

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
