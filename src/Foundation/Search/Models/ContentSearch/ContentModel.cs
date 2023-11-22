using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Classes.TypeConverters;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace OneWeb.Foundation.Search.Models.ContentSearch
{
    public class ContentModel
    {
        public virtual string this[string key]
        {
            get => key != null ? (this._fields.ContainsKey(key.ToLowerInvariant()) ? this._fields[key.ToLowerInvariant()]?.ToString() : string.Empty) : string.Empty;
            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                this._fields[key.ToLowerInvariant()] = (object)value;
            }
        }
        public virtual object this[ObjectIndexerKey key]
        {
            get => key != null ? this._fields[key.ToString().ToLowerInvariant()] : string.Empty;
            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                this._fields[key.ToString().ToLowerInvariant()] = value;
            }
        }

        [IndexField(FieldNames.ItemId)]
        public ID ItemId { get; set; }
        [IndexField(FieldNames.ItemFullPath)]
        public string ItemFullPath { get; set; }
        [IndexField(FieldNames.ItemTemplate)]
        public ID Template { get; set; }
        private readonly Dictionary<string, object> _fields = new Dictionary<string, object>();
        [IndexField(FieldNames.ItemCreated)]
        public DateTime DateCreated { get; set; }
        [IndexField(FieldNames.ItemIsLatestVersion)]
        public bool LatestVersion { get; set; }
        [IndexField(FieldNames.Date)]
        public string Date => !string.IsNullOrWhiteSpace(this[FieldNames.Date]) ? DefaultExtensions.GetDefaultDate(this[FieldNames.Date], this[FieldNames.ItemTemplate] == OneWebResource.TemplateIdString) : string.Empty;
        [IndexField(FieldNames.ItemPath)]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public virtual IEnumerable<ID> Paths { get; set; }

        private string _title;
        public string Title
        {
            get => StringExtensions.DeSanitizeSearchString(_title ?? this[FieldNames.Title]);
            set => _title = StringExtensions.DeSanitizeSearchString(value);
        }

        private string _content;

        public string Content
        {
            get => StringExtensions.DeSanitizeSearchString(_content ?? this[FieldNames.Content]);
            set => _content = StringExtensions.DeSanitizeSearchString(value);
        }
        [TypeConverter(typeof(JsonTypeConverter))]
        [IndexField(FieldNames.Icon)]
        public string Icon { get; set; }
        [IndexField(FieldNames.OneWebContent)]
        public string AllContent { get; set; }
        [IndexField(FieldNames.ItemName)]
        public string Name { get; set; }
        [IndexField(FieldNames.Image)]
        [TypeConverter(typeof(MediaModelTypeConverter))]
        public MediaModel Image { get; set; }
        [IndexField(FieldNames.OneWebTaxonomy)]
        public TaxonomyModel Taxonomy { get; set; }
        [IndexField(FieldNames.ItemTemplateName)]
        public string TemplateName { get; set; }
        [IndexField(FieldNames.HideFromSearch)]
        public bool HideFromSearch { get; set; }
        [IndexField(FieldNames.HideFromAutoSuggest)]
        public bool HideFromAutoSuggest { get; set; }
        [IndexField(FieldNames.ItemFullPath)]
        public string FullPath { get; set; }
        [IndexField(FieldNames.OneWebBusiness)]
        public string Business { get; set; }
        public string Url => ItemExtensions.GetItemUrl(this[FieldNames.ItemId], ItemLanguage);
        //public MediaModel DefaultImage => new MediaModel(DefaultExtensions.GetDefaultImageFromSettings(DefaultExtensions.GetImageFieldName(this.Template.ToString())));

        [IndexField(FieldNames.Location)]
        public string City { get; set; }
        [IndexField(FieldNames.Country)]
        public string Country { get; set; }
        [IndexField(FieldNames.BoothNo)]
        public string BoothNo { get; set; }
        public string StartDate => !string.IsNullOrWhiteSpace(this[FieldNames.StartDate])
            ? DefaultExtensions.FormatDate(this[FieldNames.StartDate], DefaultExtensions.GetDateFormat(this.Template.ToString())) : string.Empty;
        public string EndDate => !string.IsNullOrWhiteSpace(this[FieldNames.EndDate]) 
            ? DefaultExtensions.FormatDate(this[FieldNames.EndDate], DefaultExtensions.GetDateFormat(this.Template.ToString())) : string.Empty;
        [IndexField(FieldNames.Date)]
        public DateTime PublishedDate { get; set; }
        [IndexField(FieldNames.ItemLanguage)]
        public string ItemLanguage { get; set; }
        [IndexField(FieldNames.Subtext)]
        public string Subtext { get; set; }

        [IndexField(FieldNames.Designation)]
        public string Designation { get; set; }
        [IndexField(FieldNames.Caption)]
        public string Caption { get; set; }
    }
}