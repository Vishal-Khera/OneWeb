using Sitecore;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Pipelines.Response.GetPageItem;

namespace OneWeb.Foundation.Login.Pipelines
{
    public class CheckItemResolved : MvcPipelineProcessor<GetPageItemArgs>
    {
        public override void Process(GetPageItemArgs args)
        {
            var resolved = Sitecore.Context.Items["custom::ItemResolved"];
            if (MainUtil.GetBool(resolved, false))
            {
                // item has previously been resolved
                args.Result = Context.Item;
            }
        }
    }
}