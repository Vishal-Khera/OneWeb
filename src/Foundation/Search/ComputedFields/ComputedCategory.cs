using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedCategory : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem.IsMediaItem())
            {
                if (string.IsNullOrWhiteSpace(indexableItem.GetFieldValue(BaseCategory.Fields.Category_FieldName)))
                {
                    return null;
                }
                return indexableItem.GetFieldValue(BaseCategory.Fields.Category);

            }
            else
            {
                if (StringExtensions.IdEqualsGuid(indexableItem.TemplateID, Resource.TemplateIdString))
                {
                    var primaryTag = indexableItem.GetReferencedItem(Taxonomy.Fields.TaxonomyTag);
                    if (primaryTag != null)
                    {
                        return primaryTag.GetFieldValue(OneWebTag.Fields.Title_FieldName);
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(indexableItem.GetFieldValue(BaseCategory.Fields.Category)))
                        return null;
                }

                return indexableItem.GetFieldValue(BaseCategory.Fields.Category);
            }
        }
    }
}