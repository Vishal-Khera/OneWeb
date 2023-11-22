using System.Collections.Generic;
using Scriban;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Fields;
using Sitecore.XA.Foundation.Variants.Abstractions.Fields;

namespace OneWeb.Foundation.SitecoreExtensions.Models.Pipelines.XA
{
    public class VariantScriban : RenderingVariantFieldBase
    {
        public VariantScriban(Item variantItem)
            : base(variantItem)
        {
        }

        public string Template { get; set; }

        public IEnumerable<BaseVariantField> ChildItems { get; set; }

        public Template ScribanTemplate { get; set; }
        public bool IsHandlebarTemplate { get; set; }

        public string Path { get; set; }
    }
}