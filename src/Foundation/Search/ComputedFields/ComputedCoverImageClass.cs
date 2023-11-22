using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using OneWeb.Foundation.SitecoreExtensions.Extensions;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedCoverImageClass : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Item indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem == null)
                return (object)null;

            return CardExtensions.GetCoverImageClass(indexableItem);
        }
    }
}