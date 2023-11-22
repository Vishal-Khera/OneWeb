using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedContent : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            string returnValue = null;
            if (indexableItem.IsMediaItem())
            {
                if (string.IsNullOrWhiteSpace(indexableItem.GetFieldValue("Description")))
                {
                    return null;
                }
                returnValue = indexableItem.GetFieldValue("Description");

            }
            if (string.IsNullOrWhiteSpace(indexableItem.GetFieldValue(BaseContent.Fields.Content)))
                return null;
            ;
            returnValue = StringExtensions.StripAnchorTags(indexableItem.GetFieldValue(BaseContent.Fields.Content));

            return StringExtensions.SanitizeSearchString(returnValue);
        }
    }
}