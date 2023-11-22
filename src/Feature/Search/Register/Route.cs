using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace OneWeb.Feature.Search.Register
{
    public class Route
    {
        public virtual void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("OneWebSearch",
                "oneweb/{controller}/{action}",
                    new { controller = "SearchController", action = "GetSearchResults" },
                new[] { "OneWeb.Feature.Search.Controllers" }
                );
        }
    }
}