using Newtonsoft.Json;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedIcon : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            IconModel iconModel;
            var settingsItem = SiteExtensions.GetSettingsItem(indexableItem);
            if (StringExtensions.IdEqualsGuid(indexableItem.TemplateID, OneWebVideo.TemplateIdString))
            {
                iconModel = new IconModel()
                {
                    Title = Sitecore.Globalization.Translate.TextByLanguage("OneWeb-FileType-Video", indexableItem.Language, "Video"),
                    IconClass = settingsItem.GetFieldValue(SiteConfigurations.Fields.DefaultVideoIcon_FieldName)
                };
            }
            else
            {
                if (!indexableItem.IsMediaItem())
                {
                    return null;
                }

                var mediaItem = (MediaItem)indexableItem;
                switch (mediaItem.Extension)
                {
                    case "ppt":
                        iconModel = new IconModel()
                        {
                            Title = Sitecore.Globalization.Translate.TextByLanguage("OneWeb-FileType-PPT", indexableItem.Language, "PPT"),
                            IconClass = settingsItem.GetFieldValue(SiteConfigurations.Fields.DefaultPptIcon_FieldName)
                        };
                        break;
                    case "doc":
                        iconModel = new IconModel()
                        {
                            Title = Sitecore.Globalization.Translate.TextByLanguage("OneWeb-FileType-DOC", indexableItem.Language, "DOC"),
                            IconClass = settingsItem.GetFieldValue(SiteConfigurations.Fields.DefaultDocIcon_FieldName)
                        };
                        break;
                    case "pdf":
                    default:
                        iconModel = new IconModel()
                        {
                            Title = Sitecore.Globalization.Translate.TextByLanguage("OneWeb-FileType-PDF", indexableItem.Language, "PDF"),
                            IconClass = settingsItem.GetFieldValue(SiteConfigurations.Fields.DefaultPdfIcon_FieldName)
                        };
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(iconModel?.Title))
            {
                return JsonConvert.SerializeObject(iconModel);
            }

            return null;
        }
    }
}