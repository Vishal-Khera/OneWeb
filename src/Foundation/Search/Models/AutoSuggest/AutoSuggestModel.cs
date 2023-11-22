using System;
using System.Collections.Generic;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch.SearchTypes;

namespace OneWeb.Foundation.Search.Models.AutoSuggest
{
    public class AutoSuggestModel
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
        public string Date => !string.IsNullOrWhiteSpace(this[FieldNames.Date]) ? DefaultExtensions.GetDefaultDate(this[FieldNames.Date], this[FieldNames.ItemTemplate] == OneWebResource.TemplateIdString) : string.Empty;
        public string Title => StringExtensions.DeSanitizeSearchString(this[FieldNames.Title]);
        public string Content => StringExtensions.DeSanitizeSearchString(this[FieldNames.Content]);
        public string Icon => this[FieldNames.Icon];
        public string AllContent => this[FieldNames.OneWebContent];
        public string Name => this[FieldNames.ItemName];
        public string Image => this[FieldNames.Image];
        public string Taxonomy => this[FieldNames.OneWebTaxonomy];
        public string TemplateName => this[FieldNames.ItemTemplateName];
        public bool HideFromSearch => bool.Parse(this[FieldNames.HideFromSearch]);
        public bool HideFromAutoSuggest => bool.Parse(this[FieldNames.HideFromAutoSuggest]);
        public string FullPath => this[FieldNames.ItemFullPath];
        public string Business => this[FieldNames.OneWebBusiness];
        public string Url => ContextExtensions.GetContextSiteDatabase().GetItem(this[FieldNames.ItemFullPath]).GetItemUrl(null, ItemLanguage);
        public string ItemLanguage => this[FieldNames.ItemLanguage];
    }
}