using Newtonsoft.Json;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedCoverImage : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem.TemplateID != Resource.TemplateId)
            {
                return null;
            }
            var coverImage = indexableItem.GetMediaItem(BaseCoverImage.Fields.CoverImage_FieldName);
            if (coverImage == null)
            {
                coverImage = DefaultExtensions.GetDefaultCardImage(indexableItem);
            }
            return coverImage == null ? null : JsonConvert.SerializeObject(new MediaModel(coverImage));
        }
    }
}