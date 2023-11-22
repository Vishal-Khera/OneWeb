using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using StringExtensions = OneWeb.Foundation.SitecoreExtensions.Extensions.StringExtensions;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedAdvancedTags : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem != null && indexableItem.InheritsFrom(Taxonomy.TemplateId))
            {
                var advancedTagModel = new Dictionary<string, List<AdvancedTag>>();
                var allTags = indexableItem.GetMultiListItems(Taxonomy.Fields.Tags_FieldName);
                if (allTags.Any())
                {
                    foreach (var tag in allTags)
                    {
                        if (tag.Parent != null && StringExtensions.IdEqualsGuid(tag.Parent.TemplateID, OneWebTagCategory.TemplateIdString) && !string.IsNullOrWhiteSpace(tag.Parent.GetFieldValue(OneWebTagCategory.Fields.CategoryName_FieldName)))
                        {
                            var tagCategoryName =
                                tag.Parent.GetFieldValue(OneWebTagCategory.Fields.CategoryName_FieldName);
                            if (advancedTagModel.ContainsKey(tagCategoryName))
                            {
                                advancedTagModel[tagCategoryName].Add(new AdvancedTag()
                                {
                                    Title = tag.GetFieldValue(OneWebTag.Fields.Title),
                                    Id = tag.ID.ToShortID().ToString(),
                                });
                            }
                            else
                            {
                                advancedTagModel.Add(tagCategoryName, new List<AdvancedTag>(){ new AdvancedTag()
                                {
                                    Title = tag.GetFieldValue(OneWebTag.Fields.Title),
                                    Id = tag.ID.ToShortID().ToString(),
                                }});
                            }
                        }
                    }

                    return advancedTagModel;
                }
            }
            return null;
        }
    }
}