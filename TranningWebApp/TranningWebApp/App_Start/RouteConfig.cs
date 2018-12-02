﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TmsWebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Admin",
                url: "admin",
                defaults: new { controller = "Account", action = "Admin" }
            );

            routes.MapRoute(
               name: "Error",
               url: "error",
               defaults: new { controller = "Home", action = "Error" }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                // defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );


            
        }
    }
}
