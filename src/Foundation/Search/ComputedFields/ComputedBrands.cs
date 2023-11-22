using System.Collections.Generic;
using System.Linq;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedBrands : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var businessItem = (Item)(indexable as SitecoreIndexableItem);
            if (string.IsNullOrWhiteSpace(businessItem.GetFieldValue(OneWebBusiness.Fields.Brands_FieldName)))
                return null;

            return GetBrandNames(businessItem);
        }

        private List<object> GetBrandNames(Item businessItem)
        {
            var brandNames = new List<object>();
            foreach (var brandItem in businessItem.GetMultiListItems(OneWebBusiness.Fields.Brands_FieldName))
            {
             brandNames.Add(brandItem.GetFieldValue(BaseContent.Fields.Title));
            }

            return brandNames.Any() ? brandNames : null;
        }
    }
}