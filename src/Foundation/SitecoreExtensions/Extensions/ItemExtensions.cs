using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Sites;
using Sitecore.XA.Foundation.Multisite.Extensions;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class ItemExtensions
    {
        public static string ShortId(this Item item)
        {
            return item?.ID.ToShortID().ToString();
        }

        public static Item GetItem(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId)) return null;

            return Context.Database == null ? null : Context.Database.GetItem(itemId);
        }

        public static Item SelectItem(string itemPath)
        {
            if (string.IsNullOrWhiteSpace(itemPath)) return null;

            return Context.Database == null ? null : Context.Database.SelectSingleItem(itemPath);
        }

        public static Item GetItem(ID itemId)
        {
            if (itemId == ID.Null) return null;

            return Context.Database == null ? null : Context.Database.GetItem(itemId);
        }

        public static bool IsImage(this Item item)
        {
            return new MediaItem(item).MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsVideo(this Item item)
        {
            return new MediaItem(item).MimeType.StartsWith("video/", StringComparison.InvariantCultureIgnoreCase);
        }

        public static Item GetAncestorOrSelfOfTemplate(this Item item, ID templateId)
        {
            if (item == null) return null;

            return item.DescendsFrom(templateId)
                ? item
                : item.Axes.GetAncestors().LastOrDefault(i => i.DescendsFrom(templateId));
        }

        public static IList<Item> GetAncestorsAndSelfOfTemplate(this Item item, ID templateId)
        {
            if (item == null) return new List<Item>();

            var returnValue = new List<Item>();
            if (item.DescendsFrom(templateId)) returnValue.Add(item);

            returnValue.AddRange(item.Axes.GetAncestors().Reverse().Where(i => i.DescendsFrom(templateId)));
            return returnValue;
        }

        public static bool HasLayout(this Item item)
        {
            return item?.Visualization?.Layout != null;
        }

        public static bool IsDerived(this Item item, ID templateId)
        {
            if (item == null) return false;

            return !templateId.IsNull && item.IsDerived(item.Database.Templates[templateId]);
        }

        private static bool IsDerived(this Item item, Item templateItem)
        {
            if (item == null) return false;

            if (templateItem == null) return false;

            var itemTemplate = TemplateManager.GetTemplate(item);
            return itemTemplate != null &&
                   (itemTemplate.ID == templateItem.ID || itemTemplate.DescendsFrom(templateItem.ID));
        }

        public static bool HasVersionInLanguage(this Item item, Language lang = null)
        {
            if (item == null) return false;

            if (lang == null) lang = Context.Language;

            return ItemManager.GetVersions(item, lang).Any();
        }

        public static string GetItemUrl(this Item item, UrlOptions urlOptions = null, string itemLanguage = null)
        {
            var sxaSite = Context.Site;

            if (item.TemplateID.Equals(OneWebEvents.TemplateId))
            {
                LinkModel linkModel = new LinkModel(item, BaseCta.Fields.PrimaryLink_FieldName);
                return linkModel.Url;
            }
            if (item.TemplateID.Equals(OneWebMediaDownloadCard.TemplateId))
            {
                MediaModel mediaModel = new MediaModel(item, BaseMedia.Fields.Media_FieldName);
                return mediaModel.Url;
            }
            if (urlOptions != null)
            {
                return LinkManager.GetItemUrl(item, urlOptions);
            }

            var defaultUrlOptions = LinkManager.GetDefaultUrlOptions();
            if (sxaSite != null)
            {
                defaultUrlOptions.Language = string.IsNullOrWhiteSpace(itemLanguage) ? Context.Language : Language.Parse(itemLanguage);
                defaultUrlOptions.LanguageEmbedding = sxaSite.IsLanguageEmbeddingEnabled()
                    ? LanguageEmbedding.Always
                    : LanguageEmbedding.Never;
                return LinkManager.GetItemUrl(item, defaultUrlOptions);
            }

            return string.Empty;
        }

        public static string GetItemUrlWithDomain(this Item item, UrlOptions urlOptions = null)
        {
            var sxaSite = Context.Site;
            var url = GetItemUrl(item, urlOptions);
            return HttpContext.Current.Request.Url.Scheme + "://" + sxaSite.TargetHostName + url;
        }

        public static string GetItemUrl(string itemId, string itemLanguage = null)
        {
            var contextDatabase = ContextExtensions.GetContextSiteDatabase();
            if (contextDatabase != null)
            {
                var item = contextDatabase.GetItem(itemId);
                if (item == null)
                {
                    if (ShortID.TryParse(itemId, out var shortId))
                    {
                        item = contextDatabase.GetItem(ID.Parse(shortId));
                        if(item.TemplateID.Equals(OneWebEvents.TemplateId))
                        {
                            LinkModel linkModel = new LinkModel(item, BaseCta.Fields.PrimaryLink_FieldName);
                            return linkModel.Url;
                        }
                        if (item.TemplateID.Equals(OneWebMediaDownloadCard.TemplateId))
                        {
                            MediaModel mediaModel = new MediaModel(item, BaseMedia.Fields.Media_FieldName);
                            return mediaModel.Url;
                        }
                        return GetItemUrl(item, null, itemLanguage);
                    }
                }
            }

            return string.Empty;
        }

        public static bool IsMediaItem(this Item item)
        {
            return item != null && item.Paths.IsMediaItem;
        }

        public static IEnumerable<Item> GetTargetItems(Item item, string fieldName)
        {
            if (fieldName.Is(string.Empty))
                return Enumerable.Empty<Item>();
            var source = Enumerable.Empty<Item>();
            var field1 = item.Fields[fieldName];
            if (field1 != null)
            {
                var field2 = FieldTypeManager.GetField(field1);
                if (field2 is FileField)
                    source = new List<Item>
                    {
                        ((FileField)item.Fields[fieldName]).MediaItem
                    };
                if (field2 is ImageField)
                    source = new List<Item>
                    {
                        ((ImageField)item.Fields[fieldName]).MediaItem
                    };
                if (field2 is LinkField)
                    source = new List<Item>
                    {
                        ((LinkField)item.Fields[fieldName]).TargetItem
                    };
                if (field2 is ReferenceField)
                    source = new List<Item>
                    {
                        ((ReferenceField)item.Fields[fieldName]).TargetItem
                    };
                if (field2 is LookupField)
                    source = new List<Item>
                    {
                        ((LookupField)item.Fields[fieldName]).TargetItem
                    };
                if (field2 is MultilistField)
                    source = ((MultilistField)item.Fields[fieldName]).GetItems().ToList();
            }

            return source.Where(i => i != null);
        }


        public static string GetGatedUrl(Item item)
        {
            var contextDatabase = ContextExtensions.GetContextSiteDatabase();
            if (contextDatabase != null)
            {
                var mediaField = (CheckboxField)item.Fields[BaseMedia.Fields.IsGatedContent_FieldName];
                if (mediaField.Checked)
                {
                    if (HttpContext.Current.Request.Cookies["UserName"] != null)
                    {
                        string cookie = HttpContext.Current.Request.Cookies["UserName"].Value;
                        if (cookie != "")
                        {
                            LinkModel linkModel = new LinkModel(item, BaseCta.Fields.PrimaryLink_FieldName);
                            string url = linkModel.Url;
                            return url;

                        }
                        else
                        {
                            Item settings = SiteExtensions.GetSettingsItem();
                            var loginPage = settings.GetReferencedItem(SiteConfigurations.Fields.GatedLoginPage_FieldName);
                            string URL = loginPage.GetItemUrl();
                            return URL;
                        }
                    }
                    else
                    {

                        Item settings = SiteExtensions.GetSettingsItem();
                        var loginPage = settings.GetReferencedItem(SiteConfigurations.Fields.GatedLoginPage_FieldName);
                        string URL = loginPage.GetItemUrl();
                        return URL;


                    }
                }
                else
                {
                    LinkModel linkModel = new LinkModel(item, BaseCta.Fields.PrimaryLink_FieldName);
                    string url = linkModel.Url;
                    return url;
                }
            }
            return string.Empty;
        }

        public static string StripTags(Item item, string fieldName)
        {
            var content = item.Fields[fieldName].Value.ToString();
            var regex = @"<(?!\/?p(?=>|\s.*>))\/?.*?>";
            var stripedContent = Regex.Replace(content, regex, "");
            regex = @"</?[hH]3[^>]*>";
            stripedContent = Regex.Replace(stripedContent, regex, "");
            regex = @"</?[hH]2[^>]*>";
            stripedContent = Regex.Replace(stripedContent, regex, "");
            regex = @"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>";
            stripedContent = Regex.Replace(stripedContent, regex, "");
            regex = @"/\s*<ul[^>]*>[\S\s]*?<\/ul>\s*/";
            stripedContent = Regex.Replace(stripedContent, regex, "");
            return stripedContent;
        }
       
    }
}