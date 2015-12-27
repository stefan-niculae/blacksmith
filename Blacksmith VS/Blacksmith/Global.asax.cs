using System;
using System.Data.Entity;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using Blacksmith.Models;

namespace Blacksmith
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Generate Database and add initial data
            Database.SetInitializer(new DatabaseInitializer());

            // Add custom routes
//            RouteTable.Routes.Clear();
            RegisterRoutes(RouteTable.Routes);
        }

        void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(
                "LinkByAddress",
                "Link/{address}",
                "~/Link.aspx",
                checkPhysicalUrlAccess: true,
                defaults: new RouteValueDictionary { {
                        "address", string.Empty
                    } },
                constraints: new RouteValueDictionary {{
                        "address", @"([a-z0-9][a-z0-9\-]*\.)+[a-z0-9][a-z0-9\-]*"
                    } }
            );
        }
    }
}