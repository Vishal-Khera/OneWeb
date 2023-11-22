using System;
using Newtonsoft.Json.Linq;
using OneWeb.Foundation.SitecoreExtensions.Classes.TypeConverters;
using OneWeb.Foundation.SitecoreExtensions.Models;
using SolrNet.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace OneWeb.Foundation.Search.Models.SolrNetSearch
{
    public class SolrNetModel
    {
        private readonly Dictionary<string, object> _fields = new Dictionary<string, object>();
        public virtual string this[string key]
        {
            get => key != null && this._fields.ContainsKey(key.ToLowerInvariant()) ? this._fields[key.ToLowerInvariant()].ToString() : string.Empty;
            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                this._fields[key.ToLowerInvariant()] = (object)value;
            }
        }
        public virtual object this[ObjectIndexerKey key]
        {
            get => key != null && this._fields.ContainsKey(key.ToString().ToLowerInvariant()) ? this._fields[key.ToString().ToLowerInvariant()] : string.Empty;
            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                this._fields[key.ToString().ToLowerInvariant()] = value;
            }
        }

        public string ItemId => this[FieldNames.ItemId];
        public string Template => this[FieldNames.ItemTemplate];
        public string Date => !string.IsNullOrWhiteSpace(this[DefaultExtensions.GetDateIndexField(this.Template)]) ? DefaultExtensions.FormatDate(this[DefaultExtensions.GetDateIndexField(this.Template)], DefaultExtensions.GetDateFormat(this.Template)) : string.Empty;
        public string Title => StringExtensions.DeSanitizeSearchString(this[FieldNames.Title]);
        public string Content => StringExtensions.DeSanitizeSearchString(this[FieldNames.Content]);
        public string Icon => this[FieldNames.Icon];
        public string AllContent => this[FieldNames.OneWebContent];
        public string Name => this[FieldNames.ItemName];
        public string Image => this[FieldNames.Image];
        public string Taxonomy => this[FieldNames.OneWebTaxonomy];
        public string TemplateName => this[FieldNames.ItemTemplateName];
        public bool HideFromSearch => bool.Parse(!string.IsNullOrWhiteSpace(this[FieldNames.HideFromSearch]) ? this[FieldNames.HideFromSearch] : "False");
        public bool HideFromAutoSuggest => bool.Parse(!string.IsNullOrWhiteSpace(this[FieldNames.HideFromAutoSuggest]) ? this[FieldNames.HideFromAutoSuggest] : "False");
        public string FullPath => this[FieldNames.ItemFullPath];
        public string Business => this[FieldNames.OneWebBusiness];
        public string Url => ItemExtensions.GetItemUrl(this[FieldNames.ItemId], ItemLanguage);
        public string ItemLanguage => this[FieldNames.ItemLanguage];
        public string JobDepartment => this[FieldNames.JobDepartment];
        public string JobType => this[FieldNames.JobType];
        public string JobAvailability => this[FieldNames.JobAvailability];
        public string JobLocation => this[FieldNames.JobLocation];
        public string PageType => this[FieldNames.PageType];
        public DateModel EventStartDate => new DateModel(this[FieldNames.StartDate]);
        public DateModel EventEndDate => new DateModel(this[FieldNames.EndDate]);
        public string EventLocation => this[FieldNames.Location];
        public string EventCountry => this[FieldNames.Country];

        public List<AdvancedTag> Applications => _fields.ContainsKey(FieldNames.Applications)
            ? StringExtensions.ParseAdvancedTags(this[FieldNames.Applications] as string)
            : new List<AdvancedTag>();
        public List<AdvancedTag> Industries => _fields.ContainsKey(FieldNames.Industries)
            ? StringExtensions.ParseAdvancedTags(this[FieldNames.Industries] as string)
            : new List<AdvancedTag>();
    }
}