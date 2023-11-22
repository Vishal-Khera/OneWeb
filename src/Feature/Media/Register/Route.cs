using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace OneWeb.Feature.Media.Register
{
    public class Route
    {
        public virtual void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("OneWebVideo",
                "OneWeb/{controller}/{action}/{videoId}",
                    new { controller = "VideoController", action = "GetModal"},
                new[] { "OneWeb.Feature.Media.Controllers" }
                );
        }
    }
}