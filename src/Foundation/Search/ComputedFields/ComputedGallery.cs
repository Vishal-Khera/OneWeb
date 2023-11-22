using System.Collections.Generic;
using System.Linq;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedGalery : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var dateItem = (Item)(indexable as SitecoreIndexableItem);
            if (string.IsNullOrWhiteSpace(dateItem.GetFieldValue(BaseDate.Fields.Date_FieldName)))
                return null;

            return GetGalleryItems(dateItem);
        }

        private List<object> GetGalleryItems(Item dateItem)
        {
            var productTypeArray = new List<object>();
            var galleryItems = dateItem.GetChildren();
            foreach (Item galleryItem in galleryItems)
            {
                if (string.IsNullOrWhiteSpace(galleryItem.GetFieldValue(BaseImage.Fields.Image_FieldName)))
                    return null;
                productTypeArray.AddRange(galleryItem.GetMultiListItems(OneWebBusinessProduct.Fields.ProductType_FieldName)
                    .Select(productType => productType.GetFieldValue(OneWebText.Fields.Value)));
            }

            return productTypeArray.Any() ? productTypeArray.Distinct().ToList() : null;
        }
    }
}