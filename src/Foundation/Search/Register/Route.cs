using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace OneWeb.Foundation.Search.Register
{
    public class Route
    {
        public virtual void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("OneWebIndustryFilter",
                "OneWebFilter/{controller}/{action}/{mappingId}",
                    new { controller = "MappingController", action = "GetMapping"},
                new[] { "OneWeb.Foundation.Search.Controllers" }
                );
        }
    }
}