using OneWeb.Foundation.Search;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.SitecoreExtensions.Classes.TypeConverters;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace OneWeb.Feature.PageContent.Models
{
    public class SalesTabModel : ContentModel
    {
        [IndexField(FieldNames.Title)]
        public string Title { get; set; }
        [IndexField(FieldNames.JobTitle)]
        public string JobTitle { get; set; }
        [IndexField(FieldNames.PrimaryLink)]
        [TypeConverter(typeof(LinkModelTypeConverter))]
        public LinkModel Link { get; set; }

        public string Url => ItemExtensions.GetItemUrl(this[FieldNames.ItemId]);
        [IndexField(FieldNames.Image)]
        public MediaModel SalesImage { get; set; }
        [IndexField(FieldNames.Content)]
        public string Content { get; set; }
        [IndexField(FieldNames.Tags)]
        public ID[] Tags { get; set; }
        public string Combined { get; set; }
        [IndexField(FieldNames.SortOrder)]
        public int SortOrder { get; set; }
    }

    public class SalesTabTagModel : IEquatable<SalesTabTagModel>
    {
        [IndexField(FieldNames.Title)]
        public string TagTitle { get; set; }

        public bool Equals(SalesTabTagModel other)
        {
            if (TagTitle == other.TagTitle)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hashFirstName = TagTitle == null ? 0 : TagTitle.GetHashCode();
            return hashFirstName;
        }
    }
    public class SalesTabListModel : VariantsRenderingModel
    {
        public List<SalesTabModel> SalesTab { get; set; }
        public List<SalesTabTagModel> SalesTabTag { get; set; }
    }
}