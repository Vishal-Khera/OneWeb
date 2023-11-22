using OneWeb.Foundation.Search;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.SitecoreExtensions.Classes.TypeConverters;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OneWeb.Feature.PageContent.Models
{
    public class EventsModel : ContentModel
    {
        [IndexField(FieldNames.StartDate)] 
        public DateTime StartDate { get; set; }

        [IndexField(FieldNames.EndDate)]
        public DateTime EndDate { get; set; }
        [IndexField(FieldNames.Location)]
        public string Location { get; set; }
        [IndexField(FieldNames.PrimaryLink)]
        [TypeConverter(typeof(LinkModelTypeConverter))]
        public LinkModel Link { get; set; }
        [IndexField(FieldNames.Country)]
        public string Country { get; set; }
        [IndexField(FieldNames.BoothNo)]
        public string BoothNo { get; set; }

    }
    public class EventsListModel : VariantsRenderingModel
    {
        public List<EventsModel> Events { get; set; }
    }
}