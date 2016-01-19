using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Piotr.AspNetFileServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{fileName}",
                defaults: new { name = RouteParameter.Optional }
            );
        }
    }
}
