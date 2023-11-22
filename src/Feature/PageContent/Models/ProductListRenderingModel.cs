using System.Collections.Generic;
using OneWeb.Feature.PageStructure.Models;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.PageContent.Models 
{
    public class ProductListRenderingModel : VariantsRenderingModel
    {
        public SlideWrapperRenderingModel SlideWrapperSettings { get; set; }
        public List<ProductListModel> ProductList { get; set; }
    }
}