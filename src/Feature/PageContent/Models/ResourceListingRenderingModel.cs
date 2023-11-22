using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System.Collections.Generic;

namespace OneWeb.Feature.PageContent.Models
{
    public class ResourceListingRenderingModel : VariantsRenderingModel
    {
        public List<ResourceModel> Resources { get; set; }
    }
}