using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Scriban.Runtime;
using Sitecore.Abstractions;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.Multisite.Services;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddLinkFunctions : IGenerateScribanContextProcessor
    {
        protected readonly IContext Context;

        public AddLinkFunctions(
            IContext context)
        {
            Context = context;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Import("sc_link", (AddLinkDelegate)(item =>
            {
                if (item == null)
                    return "#";
                return item.GetItemUrl();
            }));
            args.GlobalScriptObject.Import("sc_medialink", (AddLinkDelegate)(mediaItem =>
            {
                if (mediaItem == null)
                    return "#";
                return mediaItem.GetMediaUrl();
            }));
        }

        private delegate string AddLinkDelegate(Item inputItem);
    }
}