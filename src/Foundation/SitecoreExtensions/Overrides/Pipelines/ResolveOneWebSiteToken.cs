using System;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.TokenResolution.Pipelines.ResolveTokens;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines
{
    public class ResolveOneWebSiteToken : ResolveTokensProcessor
    {
        public override void Process(ResolveTokensArgs args)
        {
            var query = args.Query;
            var contextItem = args.ContextItem;
            var str = this.ReplaceTokenWithValue(query, "$owSite", (Func<string>)(() => SiteExtensions.GetSiteRootByItem(contextItem)));
            args.Query = str;
        } 
    }
}