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
using System.Collections.Generic;

namespace OneWeb.Feature.PageContent.Models
{
    public class RelatedListModel : ContentModel
    {
        [IndexField(FieldNames.Date)] public DateTime Date { get; set; }

        [IndexField(FieldNames.Tags)]
        public ID[] Tags { get; set; }

        [IndexField(FieldNames.PageType)]
        public string PageType { get; set; }

        [IndexField(FieldNames.PrimaryLink)]
        [TypeConverter(typeof(LinkModelTypeConverter))]
        public LinkModelTypeConverter Link { get; set; }
        
    }
}