using Sitecore.Data.Items;
using Sitecore.XA.Foundation.DynamicPlaceholders.Models;
using System.Collections.Generic;

namespace OneWeb.Feature.PageContent.Models
{
    public class CompositeComponentRenderingModel : DynamicPlaceholdersRenderingModel
    {
        public int CompositeCount { set; get; }

        public bool HasCompositeLoop { set; get; }

        public SortedDictionary<int, Item> CompositeItems { set; get; }

        public string MessageHasLoop { set; get; }
    }
}