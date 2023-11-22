using Newtonsoft.Json;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class Link : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var taxonomyItem = (Item)(indexable as SitecoreIndexableItem);
            var link = FieldExtensions.GetLinkFieldModel(taxonomyItem,BaseNavigation.Fields.NavigationUrl_FieldName);

            return JsonConvert.SerializeObject(link);
        }
    }
}