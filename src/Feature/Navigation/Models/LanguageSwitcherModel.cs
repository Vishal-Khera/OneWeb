using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Models;

namespace OneWeb.Feature.Navigation.Models
{
    public class LanguageSwitcherModel : VariantListsRenderingModel
    {
        public Item LanguageSwitcherItem { get; set; }
        public IEnumerable<LanguageModel> Languages { get; set; }
    }
}