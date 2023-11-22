using System;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class CultureExtensions
    {
        public static string GetDateTimeAsString(DateTime dateTime, DateFormatType dateFormatType = default)
        {
            var settingsItem = SiteExtensions.GetSettingsItem();
            if (settingsItem == null) return dateTime.ToString(Context.Culture);
            var dateFormat = dateFormatType.Equals(DateFormatType.Default)
                ? settingsItem.GetFieldValue(SiteConfigurations.Fields.DefaultDateFormat_FieldName)
                : settingsItem.GetFieldValue(SiteConfigurations.Fields.NewsDateFormat_FieldName);
            return dateTime.ToString(dateFormat);
        }
    }
}