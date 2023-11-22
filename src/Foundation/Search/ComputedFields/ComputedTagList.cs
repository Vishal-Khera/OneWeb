using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedTagList : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            List<ID> tagList = new List<ID>();

            if (string.IsNullOrWhiteSpace(indexableItem.GetFieldValue(Taxonomy.Fields.Tags_FieldName)))
                return null;

            tagList.AddRange(indexableItem.GetMultiListFieldIds(Taxonomy.Fields.Tags_FieldName));

            if (!string.IsNullOrWhiteSpace(indexableItem.GetFieldValue(Taxonomy.Fields.TaxonomyTag_FieldName)))
                tagList.Add(indexableItem.GetReferencedItem(Taxonomy.Fields.TaxonomyTag_FieldName).ID);

            return tagList;
        }
    }
}