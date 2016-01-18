using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace HIS.Web.UI.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            RouteTable.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "app/{controller}/{action}"
             
            );
            
        }
    }
}