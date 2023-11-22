using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.HeroBanner.Model
{
    public class HeroBannerModel : VariantsRenderingModel
    {
        public Item DatasourceItem { get; set; }
        public string TemplateName { get; set; }
    }
}