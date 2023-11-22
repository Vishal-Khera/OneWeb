using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedTitle : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            string returnString = null;
            if (indexableItem.IsMediaItem())
            {
                if (string.IsNullOrWhiteSpace(indexableItem.GetFieldValue("Title")))
                {
                    return null;
                }
                returnString = indexableItem.GetFieldValue("Title");

            }
            if (string.IsNullOrWhiteSpace(indexableItem.GetFieldValue(BaseContent.Fields.Title)))
                return null;
;
            returnString = indexableItem.GetFieldValue(BaseContent.Fields.Title);

            return StringExtensions.SanitizeSearchString(returnString);
        }
    }
}