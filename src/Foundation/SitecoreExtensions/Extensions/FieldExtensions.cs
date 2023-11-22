using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneWeb.Foundation.SitecoreExtensions.Classes.Fields;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class FieldExtensions
    {
        public enum LinkFieldOption
        {
            Text,
            LinkType,
            Class,
            Alt,
            Target,
            QueryString
        }

        public static string GetFieldValue(this Item item, string fieldName)
        {
            if (item?.Fields[fieldName] == null) return string.Empty;
            return item.Fields[fieldName].Value;
        }

        public static string GetFieldValue(this Item item, ID fieldId)
        {
            if (item?.Fields[fieldId] == null) return string.Empty;
            return item.Fields[fieldId].Value;
        }

        public static DateTime GetDateValue(this Item item, ID fieldId)
        {
            if (item?.Fields[fieldId] == null || !(FieldTypeManager.GetField(item.Fields[fieldId]) is DateField dateField)) return DateTime.MinValue;
            return dateField.DateTime;
        }

        public static DateTime GetDateValue(this Item item, string fieldName)
        {
            if (item?.Fields[fieldName] == null || !(FieldTypeManager.GetField(item.Fields[fieldName]) is DateField dateField)) return DateTime.MinValue;
            return dateField.DateTime;
        }

        public static string GetEscapedFieldValue(this Item item, string fieldName)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Fields[fieldName]?.Value)) return string.Empty;
            return item.Fields[fieldName].Value.Replace("\\", "\\\\");
        }

        public static bool GetCheckboxStatus(this Item item, string fieldName)
        {
            var field = item?.Fields[fieldName];
            if (field == null || !(FieldTypeManager.GetField(field) is CheckboxField)) return false;

            return MainUtil.GetBool(field.Value, false);
        }
        public static bool GetCheckboxStatus(this Item item, ID fieldId)
        {
            var field = item?.Fields[fieldId];
            if (field == null || !(FieldTypeManager.GetField(field) is CheckboxField)) return false;

            return MainUtil.GetBool(field.Value, false);
        }

        public static IEnumerable<ID> GetMultiListFieldIds(this Item item, string fieldName)
        {
            var fieldValues = GetMultiListFieldValues(item, fieldName);
            return fieldValues.Any() ? fieldValues.Select(x => item.Database.GetItem(x).ID) : new List<ID>();
        }

        public static IEnumerable<Item> GetMultiListItems(this Item item, string fieldName)
        {
            var fieldValues = GetMultiListFieldValues(item, fieldName);
            return fieldValues.Any() ? fieldValues.Select(x => item.Database.GetItem(x)).Where(x => x.HasVersionInLanguage()) : new List<Item>();
        }

        public static IEnumerable<Item> GetMultiListItems(this Item item, ID fieldId)
        {
            var fieldValues = GetMultiListFieldValues(item, fieldId);
            return fieldValues.Any() ? fieldValues.Select(x => item.Database.GetItem(x)).Where(x => x.HasVersionInLanguage()) : new List<Item>();
        }

        public static Item GetReferencedItem(this Item item, string fieldName)
        {
            if (item == null) return null;

            var field = item.Fields[fieldName];
            if (string.IsNullOrEmpty(field?.Value)) return null;

            return FieldTypeManager.GetField(field) is ReferenceField 
                   || FieldTypeManager.GetField(field) is LookupField
                   || FieldTypeManager.GetField(field) is ValueLookupField
                   || FieldTypeManager.GetField(field) is InternalLinkField 
                ? ((ReferenceField)field).TargetItem : null;
        }

        public static Item GetReferencedItem(this Item item, ID fieldId)
        {
            if (item == null) return null;

            var field = item.Fields[fieldId];
            if (string.IsNullOrEmpty(field?.Value)) return null;

            return FieldTypeManager.GetField(field) is ReferenceField 
                   || FieldTypeManager.GetField(field) is LookupField
                   || FieldTypeManager.GetField(field) is ValueLookupField
                   || FieldTypeManager.GetField(field) is InternalLinkField 
                ? ((ReferenceField)field).TargetItem : null;
        }

        public static double? GetDouble(this Item item, ID fieldId)
        {
            var value = item?.Fields[fieldId]?.Value;
            if (value == null) return null;

            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var num) ||
                double.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out num) ||
                double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out num)) return num;
            return null;
        }

        public static bool FieldHasValue(this Item item, ID fieldId)
        {
            return item?.Fields[fieldId] != null && !string.IsNullOrWhiteSpace(item.Fields[fieldId].Value);
        }

        public static int GetInteger(this Item item, ID fieldId)
        {
            return !int.TryParse(item?.Fields[fieldId].Value, out var result) ? 0 : result;
        }

        public static string[] GetMultiListFieldValues(this Item item, string fieldName)
        {
            var fieldValues = new string[] { };
            if (item == null) return fieldValues;

            var field = item.Fields[fieldName];
            if (string.IsNullOrEmpty(field?.Value))
                return fieldValues;

            return field.Value.Split('|');
        }

        public static string[] GetMultiListFieldValues(this Item item, ID fieldId)
        {
            var fieldValues = new string[] { };
            if (item == null) return fieldValues;

            var field = item.Fields[fieldId];
            if (string.IsNullOrEmpty(field?.Value))
                return fieldValues;
            return field.Value.Split('|');
        }

        public static LinkModel GetLinkFieldModel(this Item item, string fieldName)
        {
            return new LinkModel(item, fieldName);
        }

        public static LinkModel GetLinkFieldModel(this Item item, ID fieldName)
        {
            return new LinkModel(item, fieldName);
        }

        public static Item GetMediaItem(this Item item, string fieldName)
        {
            var field = item?.Fields[fieldName];
            if (field != null)
                return FieldTypeManager.GetField(field) is ImageField ? ((ImageField)field).MediaItem : null;

            return null;
        }

        public static string GetLinkFieldUrl(LinkField lf)
        {
            try
            {
                switch (lf.LinkType.ToLower())
                {
                    case "internal":
                        // Use LinkMananger for internal links, if link is not empty

                        if (lf.TargetItem != null)
                        {
                            var itemUrl = lf.TargetItem.GetItemUrl();
                            if (!string.IsNullOrEmpty(lf.Anchor)) itemUrl += "#" + lf.Anchor;

                            if (!string.IsNullOrEmpty(lf.QueryString)) itemUrl += "?" + lf.QueryString;

                            return itemUrl;
                        }
                        else
                        {
                            return string.Empty;
                        }

                    case "media":
                        // Use MediaManager for media links, if link is not empty
                        return lf.TargetItem != null ? MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
                    case "external":
                        // Just return external links
                        return lf.Url;
                    case "anchor":
                        // Prefix anchor link with # if link if not empty
                        return !string.IsNullOrEmpty(lf.Anchor) ? "#" + lf.Anchor : string.Empty;
                    case "mailto":
                        // Just return mailto link
                        return lf.Url;
                    case "javascript":
                        // Just return javascript
                        return lf.Url;
                    default:
                        // Just please the compiler, this
                        // condition will never be met
                        return lf.Url;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string LinkFieldOptions(this Item item, ID fieldId, LinkFieldOption option)
        {
            XmlField field = item.Fields[fieldId];
            switch (option)
            {
                case LinkFieldOption.LinkType:
                    return field?.GetAttribute("linktype");
                case LinkFieldOption.Class:
                    return field?.GetAttribute("class");
                case LinkFieldOption.Alt:
                    return field?.GetAttribute("title");
                case LinkFieldOption.Target:
                    return field?.GetAttribute("target");
                case LinkFieldOption.QueryString:
                    return field?.GetAttribute("querystring");
                case LinkFieldOption.Text:
                default:
                    return field?.GetAttribute("text");
            }
        }

        public static Item TargetItem(this Item item, ID linkFieldId)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (item.Fields[linkFieldId] == null || !item.Fields[linkFieldId].HasValue) return null;
            return ((LinkField)item.Fields[linkFieldId]).TargetItem ??
                   ((ReferenceField)item.Fields[linkFieldId]).TargetItem;
        }

        public static string FetchImageAsSVG(Item item, string fieldName, string cssClass = "")
        {
            if (item == null || string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }
            ImageField field = item.Fields[fieldName];
            if (field?.MediaItem == null)
            {
                return string.Empty;
            }
            var mediaItem = new MediaItem(field.MediaItem);
            if (mediaItem.MimeType != "image/svg+xml")
            {
                return $"<!--This MediaItem name :{item.ID} is not SVG image.-->";
            }
            string result = string.Empty;
            var media = MediaManager.GetMedia(mediaItem);
            if (media?.GetStream() != null)
            {
                using (var reader = new StreamReader(media.GetStream().Stream))
                {
                    result = reader.ReadToEnd();
                }
                var svg = XDocument.Parse(result);
                if (svg.Document?.Root != null)
                {
                    svg.Document.Root.SetAttributeValue("class", cssClass);
                    result = svg.ToString();
                }
                return result;
            }

            return string.Empty;
        }

        public static Dictionary<string, string> GetOneWebNameValueList(this Item item, string fieldName)
        {
            var returnObject = new Dictionary<string, string>();
            if (item?.Fields[fieldName] != null)
            {
                var field = item.Fields[fieldName];
                var list = FieldTypeManager.GetField(field);
                // if (StringExtensions.IdEqualsGuid(list.InnerField.ID, "ed7d9005-27d5-4cf1-bec0-6018bafa68f0"))
                // {
                if (!string.IsNullOrWhiteSpace(field.Value))
                {
                    var controlObject = JsonConvert.DeserializeObject<JObject>(field.Value);
                    foreach (var property in controlObject.Properties())
                    {
                        returnObject.Add(property.Name, (string)property.Value);
                    }
                }
                //    }

            }

            //return new Dictionary<string, string>();
            return returnObject;
        }
    }
}