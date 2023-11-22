using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Theming.Configuration;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Bundler
{
    public class AssetBundler : Sitecore.XA.Foundation.Theming.Bundler.AssetBundler
    {
        public override List<Item> GetThemeChildItems(
            Item theme,
            OptimizationType optimizationType)
        {
            List<Item> themeChildItems = new List<Item>();
            Item obj = theme.Children.SingleOrDefault<Item>((Func<Item, bool>)(c => c.Name.ToLowerInvariant().Is(optimizationType.ToString().ToLowerInvariant())));
            if (obj != null)
                themeChildItems.AddRange(((IEnumerable<Item>)obj.Axes.GetDescendants()).Where<Item>((Func<Item, bool>)(item => item["Extension"].Is(optimizationType == OptimizationType.Scripts ? "js" : "css"))));
            return themeChildItems;
        }
    }
}
