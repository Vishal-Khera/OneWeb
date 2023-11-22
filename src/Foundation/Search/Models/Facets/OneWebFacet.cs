using System.Collections.Generic;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.Models.Facets
{
    public class OneWebFacet
    {
        public string Identifier { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public bool IsDynamicFilter { get; set; }
        public IList<OneWebFacetValue> Values { get; set; }
        public OneWebFacet(Item item)
        {
            this.Identifier = item.ShortId();
            this.Key = item.GetFieldValue(OneWebSearchFilter.Fields.FieldName);
            this.Name = item.GetFieldValue(OneWebSearchFilter.Fields.DisplayName);
            this.IsDynamicFilter = item.GetCheckboxStatus(OneWebSearchFilter.Fields.IsDynamicFilter_FieldName);
            this.Values = (IList<OneWebFacetValue>)new List<OneWebFacetValue>();
        }
    }
}