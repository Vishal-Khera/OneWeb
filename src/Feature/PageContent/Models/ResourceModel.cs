using Newtonsoft.Json.Linq;
using OneWeb.Foundation.Search.Models;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.SitecoreExtensions.Classes.TypeConverters;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using System;
using System.ComponentModel;
using OneWeb.Foundation.Search;
using Sitecore.Data;

namespace OneWeb.Feature.PageContent.Models
{
    public class ResourceModel : ContentModel
    {
        [IndexField(FieldNames.Date)] public DateTime Date { get; set; }

        [IndexField(FieldNames.Image)]
        [TypeConverter(typeof(MediaModelTypeConverter))]
        public MediaModel Image { get; set; }

        [IndexField(FieldNames.CoverImage)]
        [TypeConverter(typeof(MediaModelTypeConverter))]
        public MediaModel CoverImage { get; set; }

        [IndexField(FieldNames.NavigationUrl)]
        [TypeConverter(typeof(LinkModelTypeConverter))]
        public LinkModel Link { get; set; }

        [IndexField(FieldNames.OneWebTaxonomy)]
        [TypeConverter(typeof(TaxonomyTypeConverter))]
        public TaxonomyModel Taxonomy { get; set; }
        [IndexField(FieldNames.Tags)]
        public ID[] Tags { get; set; }

        [IndexField(FieldNames.CoverImageClass)]
        public string CoverImageClass { get; set; }
    }
}