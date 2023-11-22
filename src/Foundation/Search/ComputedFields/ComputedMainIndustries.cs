using System;
using System.Collections.Generic;
using System.Linq;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedMainIndustries : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var businessItem = (Item)(indexable as SitecoreIndexableItem);
            if (string.IsNullOrWhiteSpace(businessItem.GetFieldValue(OneWebBusiness.Fields.Industries_FieldName)))
                return null;

            return GetIndustries(businessItem);
        }

        private List<object> GetIndustries(Item businessItem)
        {
            var industryArray = new List<object>();
            var industryItems = businessItem.GetMultiListItems(OneWebBusiness.Fields.Industries_FieldName);
            if (industryItems.Any())
            {               
                foreach (var industryItem in industryItems)
                {
                    industryArray.Add(industryItem.Parent.GetFieldValue(BaseContent.Fields.Title_FieldName));
                }                
            }

            return industryArray.Any() ? industryArray.Distinct().ToList() : null;
        }
    }
}