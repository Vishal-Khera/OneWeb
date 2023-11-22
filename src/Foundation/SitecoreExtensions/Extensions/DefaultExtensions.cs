using System;
using System.Globalization;
using OneWeb.Foundation.Serialization;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class DefaultExtensions
    {
        public static Item GetDefaultVideoPopupVariant(Item item = null)
        {
            var settingsItem = SiteExtensions.GetSettingsItem(item);
            return settingsItem?.GetReferencedItem(SiteConfigurations.Fields.VideoPopupVariant_FieldName);
        }

        public static Item GetDefaultImage(Item item = null)
        {
            var settingsItem = SiteExtensions.GetSettingsItem(item);
            return settingsItem?.GetMediaItem(SiteConfigurations.Fields.DefaultHeroBannerImage_FieldName);
        }

        public static Item GetDefaultMobileImage(Item item = null)
        {
            var settingsItem = SiteExtensions.GetSettingsItem(item);
            return settingsItem?.GetMediaItem(SiteConfigurations.Fields.DefaultHeroBannerMobileImage_FieldName);
        }

        public static Item GetDefaultCardImage(Item item = null)
        {
            var settingsItem = SiteExtensions.GetSettingsItem(item);
            return settingsItem?.GetMediaItem(SiteConfigurations.Fields.DefaultResourceCardImage_FieldName);
        }
        public static Item GetDefaultPlayIcon()
        {
            var settingsItem = SiteExtensions.GetSettingsItem();
            return settingsItem?.GetMediaItem(SiteConfigurations.Fields.DefaultPlayImage_FieldName);
        }
        public static Item GetDefaultImageFromSettings(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return null;
            var settingsItem = SiteExtensions.GetSettingsItem();
            return settingsItem?.GetMediaItem(fieldName);
        }

        public static int GetDefaultSearchPageSize()
        {
            return 10;
        }

        public static string GetDefaultDate(this Item item, string dateFieldName, bool isNewsFormat)
        {
            return GetDefaultDate(item, dateFieldName, isNewsFormat ? "news" : "default");
        }

        public static string GetDefaultDate(string dateString, bool isNewsFormat)
        {
            return GetDefaultDate(dateString, isNewsFormat ? "news" : "default");
        }

        public static string FormatDate(string input, string format)
        {
            if (!(input is string fieldValue)) return string.Empty;
            {
                return DateTime.TryParse(fieldValue, out var dateValue) ? (string.IsNullOrEmpty(format) ? dateValue.ToString() : dateValue.ToString(format)) : string.Empty;
            }
        }

        public static string GetDateIndexField(string template)
        {
            if (string.IsNullOrEmpty(template))
                return string.Empty;
            Guid.TryParse(template, out Guid templateId);
            string Id = templateId.ToString();
            switch (Id)
            {
                case OneWebEvents.TemplateIdString:
                    return "start_date_tdt";
                default:
                    return "date_tdt";
            }
        }

        public static string GetDateFormat(string template)
        {
            if (string.IsNullOrEmpty(template))
                return string.Empty;
            Guid.TryParse(template, out Guid templateId);
            string Id = templateId.ToString();
            var settings = SiteExtensions.GetSettingsItem();
            switch (Id)
            {
                case OneWebEvents.TemplateIdString:
                    return settings[SiteConfigurations.Fields.EventsDateFormat_FieldName];
                case OneWebResource.TemplateIdString:
                    return settings[SiteConfigurations.Fields.NewsDateFormat_FieldName];
                default:
                    return settings[SiteConfigurations.Fields.DefaultDateFormat_FieldName];
            }
        }

        public static string GetDefaultDate(object input, string param1 = null, string param2 = null)
        {
            if (input == null)
                return string.Empty;

            var settingItem = SiteExtensions.GetSettingsItem();
            var newsFormat = settingItem.Fields[SiteConfigurations.Fields.NewsDateFormat].Value;
            var defaultFormat = settingItem.Fields[SiteConfigurations.Fields.DefaultDateFormat].Value;
            if (input is Item item && !string.IsNullOrWhiteSpace(param1))
            {
                var isNewsFormat = !string.IsNullOrEmpty(param2) && string.Equals(param2, "news", StringComparison.InvariantCultureIgnoreCase);
                var field = item.Fields[param1];
                if (field == null || !(FieldTypeManager.GetField(field) is DateField dateField))
                    return string.Empty;

                return dateField.DateTime.ToString(isNewsFormat ? newsFormat : defaultFormat, CultureInfo.InvariantCulture);
            }

            if (!(input is string fieldValue)) return string.Empty;
            {
                var isNewsFormat = !string.IsNullOrEmpty(param1) && string.Equals(param1, "news", StringComparison.InvariantCultureIgnoreCase);
                return DateTime.TryParse(fieldValue, out var dateValue) ? (isNewsFormat ? dateValue.ToString(newsFormat, CultureInfo.InvariantCulture) : dateValue.ToString(defaultFormat, CultureInfo.InvariantCulture)) : string.Empty;
            }
        }
        public static string GetDefaultDateValue(Item item, string fieldName, string formatFieldName)
        {
            var settings = SiteExtensions.GetSettingsItem();
            string dateFormat = settings[formatFieldName];
            var field = item.Fields[fieldName];
            if (field == null || !(FieldTypeManager.GetField(field) is DateField dateField))
                return string.Empty;
            return dateField.DateTime.ToString(dateFormat, CultureInfo.InvariantCulture);
        }

        public static string GetEventCardDateValue(Item item, string fieldName)
        {            
            string startDateFormat = "dd";
            string endDateFormat = "dd MMM yyyy";
            string firstFieldName = "StartDateTime";
            string secondFieldName = "EndDateTime";
            var fieldFirst = item.Fields[firstFieldName];
            var fieldSecond = item.Fields[secondFieldName];
            if (fieldFirst == null || !(FieldTypeManager.GetField(fieldFirst) is DateField dateFieldFirst))
                return string.Empty;
            if (fieldSecond == null || !(FieldTypeManager.GetField(fieldSecond) is DateField dateFieldSecond))
                return string.Empty;

            if (fieldName == "StartDate")
            {
                if (dateFieldFirst.DateTime.Month == dateFieldSecond.DateTime.Month && dateFieldFirst.DateTime.Year == dateFieldSecond.DateTime.Year)
                {
                    return dateFieldFirst.DateTime.ToString(startDateFormat);
                }
                else
                {
                    return dateFieldFirst.DateTime.ToString(endDateFormat);
                }
            }

            else
            {
                return dateFieldSecond.DateTime.ToString(endDateFormat);
            }
        }

        public static string GetDefaultVideoContent(string fieldName)
        {
            if (fieldName == null && string.IsNullOrEmpty(fieldName))
                return string.Empty;
            var settings = SiteExtensions.GetSettingsItem();
            string videoContent = settings[fieldName];
            return videoContent;
        }

        public static string GetImageFieldName(string template)
        {
            if(Guid.TryParse(template, out Guid templateId))
            {
                string templateItemID = templateId.ToString();
                switch (templateItemID)
                {
                    case OneWebResource.TemplateIdString:
                        return SiteConfigurations.Fields.DefaultNewsThumbnail_FieldName;
                    default:
                        return string.Empty;
                }
            }
            return string.Empty;    
        }

        public static int GetDefaultDisplayTagCount()
        {
            var settings = SiteExtensions.GetSettingsItem();
            var displayTagCount = settings.GetInteger(TaxonomyConfigurations.Fields.DefaultDisplayTagCount);
            return displayTagCount;
        }
    }
}