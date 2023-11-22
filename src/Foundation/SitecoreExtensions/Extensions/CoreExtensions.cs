using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.Pipelines;
using Sitecore.XA.Foundation.TokenResolution.Pipelines.ResolveTokens;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class CoreExtensions
    {
        public static string TriggerResolveTokenPipeline(string query, Item item)
        {
            var args = new ResolveTokensArgs(query, item);
            CorePipeline.Run("resolveTokens", args);
            return ItemExtensions.GetItem(args.Query.Replace("query:",string.Empty))?.ID?.ToString();
        }
    }
}