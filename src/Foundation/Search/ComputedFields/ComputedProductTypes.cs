using System.Collections.Generic;
using System.Linq;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedProductTypes : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var businessItem = (Item)(indexable as SitecoreIndexableItem);
            if (string.IsNullOrWhiteSpace(businessItem.GetFieldValue(OneWebBusiness.Fields.Products_FieldName)))
                return null;

            return GetProductTypes(businessItem);
        }

        private List<object> GetProductTypes(Item businessItem)
        {
            var productTypeArray = new List<object>();
            var businessProducts = businessItem.GetMultiListItems(OneWebBusiness.Fields.Products_FieldName);
            foreach (var product in businessProducts)
            {
                if (string.IsNullOrWhiteSpace(product.GetFieldValue(OneWebBusinessProduct.Fields.ProductType_FieldName)))
                    return null;
                productTypeArray.AddRange(product.GetMultiListItems(OneWebBusinessProduct.Fields.ProductType_FieldName)
                    .Select(productType => productType.GetFieldValue(OneWebText.Fields.Value)));
            }

            return productTypeArray.Any() ? productTypeArray.Distinct().ToList() : null;
        }
    }
}