using Newtonsoft.Json;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedTimeline : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (!StringExtensions.IdEqualsGuid(indexableItem.TemplateID, Resource.TemplateIdString))
                return null;

            if (string.IsNullOrWhiteSpace(indexableItem.GetFieldValue(OneWebResource.Fields.Date)))
                return null;

            return indexableItem.GetDateValue(OneWebResource.Fields.Date).Year;
        }
    }
}