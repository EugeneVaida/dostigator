using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dostigator
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "TimeLines",
                url: "Profile/TimeLines/{action}/{id}",
                defaults: new { controller = "TimeLines", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Aims",
                url: "Profile/Aims/{action}/{id}",
                defaults: new { controller = "Aims", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            

        }
    }
}
