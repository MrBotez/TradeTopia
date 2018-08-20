using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TradeTopia.Hub.Windows.Service.WebAPI.Startup))]

namespace TradeTopia.Hub.Windows.Service.WebAPI
{
  public class Startup
  {
    // This code configures Web API. The Startup class is specified as a type
    // parameter in the WebApp.Start method.
    public void Configuration(IAppBuilder appBuilder)
    {
      appBuilder.Use(typeof(MiddleWare.AuthenticationMiddleware));
      appBuilder.Use(typeof(MiddleWare.LoggingMiddleWare));

      // Configure Web API for self-host. 
      HttpConfiguration config = new HttpConfiguration();
      config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );

      appBuilder.UseWebApi(config);
    }
  }
}
