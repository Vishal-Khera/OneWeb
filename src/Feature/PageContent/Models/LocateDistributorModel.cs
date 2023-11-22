using OneWeb.Foundation.Search;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.SitecoreExtensions.Classes.TypeConverters;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OneWeb.Feature.PageContent.Models
{
    public class LocateDistributorModel : ContentModel
    {
        [IndexField(FieldNames.ZipCodeMaxRange)]
        public int ZipCodeMaxRange { get; set; }

        [IndexField(FieldNames.ZipCodeMinRange)]
        public int ZipCodeMinRange { get; set; }
        [IndexField(FieldNames.Latitude)]
        public string Latitude { get; set; }
        [IndexField(FieldNames.Longitude)]

        public string Longitude { get; set; }

        [IndexField(FieldNames.Country)]
        public string Country { get; set; }

        [IndexField(FieldNames.City)]
        public string City { get; set; }

        [IndexField(FieldNames.Mobile)]
        public string Mobile { get; set; }

        [IndexField(FieldNames.Email)]
        public string Email { get; set; }

        [IndexField(FieldNames.Image)]
        public string Image { get; set; }

        [IndexField(FieldNames.Title)]
        public string Title { get; set; }

        [IndexField(FieldNames.Content)]
        public string Content { get; set; }

        [IndexField(FieldNames.Subtext)]
        public string Subtext { get; set; }

    }

    public class LocateDistributorListModel : VariantsRenderingModel
    {
        public List<LocateDistributorModel> Events { get; set; }
    }
}