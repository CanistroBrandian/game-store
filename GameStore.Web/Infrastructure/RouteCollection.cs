using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace GameStore.Web.Infrastructure
{
    public static class RouteCollection
    {
        public static void BuildRouter(IRouteBuilder routes)
        {
            routes.MapRoute(null,
                "",
                new
                {
                    controller = "Game",
                    action = "List",
                    category = (string)null,
                    page = 1
                }
            );

            routes.MapRoute(
                name: null,
                template: "Page{page}",
                defaults: new { controller = "Game", action = "List", category = (string)null },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(null,
                "{category}",
                new { controller = "Game", action = "List", page = 1 }
            );

            routes.MapRoute(null,
                "{category}/Page{page}",
                new { controller = "Game", action = "List" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
