using Newtonsoft.Json;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedTaxonomy : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (string.IsNullOrWhiteSpace(indexableItem.GetFieldValue(Taxonomy.Fields.Tags_FieldName))  && string.IsNullOrWhiteSpace(indexableItem.GetFieldValue(Taxonomy.Fields.TaxonomyTag_FieldName)))
                return null;
;
            return JsonConvert.SerializeObject(new TaxonomyModel(indexableItem));
        }
    }
}