using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedAlphabet : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (!StringExtensions.IdEqualsGuid(indexableItem.TemplateID, OneWebBusiness.TemplateIdString) || string.IsNullOrWhiteSpace(indexableItem.GetFieldValue(BaseContent.Fields.Title_FieldName)))
            {
                return null;
            }

            return indexableItem.GetFieldValue(BaseContent.Fields.Title_FieldName).ToLowerInvariant().Trim();
        }
    }
}