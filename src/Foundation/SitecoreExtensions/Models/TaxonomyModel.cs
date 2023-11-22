using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

namespace OneWeb.Foundation.SitecoreExtensions.Models
{
    public class TaxonomyModel
    {

        public TaxonomyModel(Item item)
        {
            Tags = new List<TagModel>();
            TaxonomyTag = new TagModel();
            GetTags(item);
            GetTaxonomyTag(item);
        }

        public TagModel TaxonomyTag { get; set; }
        public List<TagModel> Tags { get; set; }

        private void GetTags(Item item)
        {
            var tags = item.GetMultiListItems(Taxonomy.Fields.Tags).ToList();
            foreach (var tag in tags)
            {
                var tagModel = new TagModel()
                {
                    Title = tag.GetFieldValue(OneWebTag.Fields.Title_FieldName),
                    Color = tag.GetFieldValue(OneWebTag.Fields.Color_FieldName)?.ToLower(),
                    Identifier = tag.ShortId().ToLower()
                };
                Tags.Add(tagModel);
            }
        }

        private void GetTaxonomyTag(Item item)
        {
            var tag = item.GetReferencedItem(Taxonomy.Fields.TaxonomyTag_FieldName);
            if (tag != null)
            {
                TaxonomyTag = new TagModel()
                {
                    Title = tag.GetFieldValue(OneWebTag.Fields.Title_FieldName),
                    Color = tag.GetFieldValue(OneWebTag.Fields.Color_FieldName)?.ToLower(),
                    Identifier = tag.ShortId().ToLower()
                };
            }
        }
    }
}