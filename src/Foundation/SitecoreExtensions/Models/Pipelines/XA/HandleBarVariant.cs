using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Fields;

namespace OneWeb.Foundation.SitecoreExtensions.Models.Pipelines.XA
{
    public class HandleBarVariant : RenderingVariantFieldBase
    {
        public HandleBarVariant(Item variantItem)
            : base(variantItem)
        {
        }

        public string Template { get; set; }
    }
}