﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Warsztat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // will allow for Cart/ChangeQuantity/2/1
            routes.MapRoute(
                    "ChangeProductQuantityOnCard",
                    "Cart/ChangeQuantity/{id}/{quantity}",
                    new { controller = "Cart", action = "ChangeQuantity" }
            );
        }
    }
}
