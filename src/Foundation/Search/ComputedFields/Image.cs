using Newtonsoft.Json;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using System;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class Image : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            string imageDetails = string.Empty;
            try
            {
                var indexableItem = indexable as SitecoreIndexableItem;
               
                if (indexableItem !=null && !indexableItem.Item.ID.IsNull && indexableItem.Item.Fields[BaseImage.Fields.Image_FieldName] != null
                    && !String.IsNullOrEmpty(indexableItem.Item.Fields[BaseImage.Fields.Image_FieldName].Value))
                {
                    var img = MediaExtensions.GetMediaModel(indexableItem.Item, BaseImage.Fields.Image_FieldName);
                    if (img != null)
                    {
                        img.Url = img.Url.Replace("/sitecore/shell", "");
                        imageDetails = JsonConvert.SerializeObject(img);
                    }
                }
            }
            catch (Exception exception)
            {
                Sitecore.Diagnostics.Log.Error("CardComponent > oneweb.Solr ComputeFieldValue", exception, "ComputeFieldValue");
            }
            return imageDetails;
        }
    }
}