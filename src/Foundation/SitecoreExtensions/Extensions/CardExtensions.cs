using OneWeb.Foundation.Serialization;
using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.Pipelines;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.TokenResolution.Pipelines.ResolveTokens;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class CardExtensions
    {
        public static string GetCoverImageClass(Item item)
        {
            if (item == null || !item.InheritsFrom(BaseCoverImageOptions.TemplateId))
            {
                return null;
            }

            var classNames = item.GetReferencedItem(BaseCoverImageOptions.Fields.ImagePosition)
                ?.GetFieldValue(OneWebText.Fields.Value);
            if (item.GetCheckboxStatus(BaseCoverImageOptions.Fields.InvertShadeInMobile_FieldName))
            {
                classNames += " reverse-strip";
            }

            return classNames;
        }
    }
}