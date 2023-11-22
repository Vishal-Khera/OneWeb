using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System.Collections.Generic;

namespace OneWeb.Feature.PageContent.Models
{
    public class RelatedListRenderingModel : VariantsRenderingModel
    {
        public List<RelatedListModel> RelatedListing { get; set; }
    }
}