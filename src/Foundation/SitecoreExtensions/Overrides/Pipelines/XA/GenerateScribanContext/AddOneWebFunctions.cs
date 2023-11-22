using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Scriban.Runtime;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using OneWeb.Foundation.SitecoreExtensions.Caching;
using System.Collections.Generic;
using System.Linq;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddOneWebFunctions : IGenerateScribanContextProcessor
    {
        private delegate MediaModel MediaModelDelegate(Item item, object type);
        private delegate LinkModel LinkModelDelegate(Item item, object type);
        private delegate VideoModel VideoModelDelegate(Item item);
        private delegate MediaModel DefaultMediaModelDelegate(object type);
        private delegate string LinkClassDelegate(object type1, object type2, object type3 = null);
        private delegate string DateFormatDelegate(object type, string param1 = null, string param2 = null);
        private delegate MediaModel DefaultVideoModelDelegate(object type);
        private delegate string SVGImageDelegate(object type1, string type2 = null, string type3 = null);
        private delegate Item ItemDelegate(string item);
        private delegate MediaModel DefaultMediaModelByFieldNameDelegate(string fieldName);
        private delegate string DefaultDateDelegate(Item item, string fieldName, string formatFieldName);
        private delegate LinkModel LinkFromSettingsDelegate(string fieldName);
        private delegate string CoverImageClassDelegate(Item item);
        private delegate string DefaultVideoContentDelegate(string fieldName);
        private delegate string GetGatedContentDelegate(Item item);
        private delegate string DescriptionWithoutTag(Item item, string fieldName);
        private delegate string EventCardDateDelegate(Item item, string fieldName);
        private delegate string DefaultPageUrl(string fieldName);

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            var defaultVideoModel = new DefaultVideoModelDelegate(GetDefaultPlayIcon);
            args.GlobalScriptObject.Import("ow_default_play_icon", defaultVideoModel);

            var mediaModel = new MediaModelDelegate(GetMediaModel);
            args.GlobalScriptObject.Import("ow_media_model", mediaModel);

            var defaultMediaModel = new DefaultMediaModelDelegate(GetDefaultMediaModel);
            args.GlobalScriptObject.Import("ow_default_media", defaultMediaModel);

            var defaultMediaModelMobile = new DefaultMediaModelDelegate(GetDefaultMediaModelMobile);
            args.GlobalScriptObject.Import("ow_default_media_mobile", defaultMediaModelMobile);


            var linkModel = new LinkModelDelegate(GetLinkModel);
            args.GlobalScriptObject.Import("ow_link_model", linkModel);

            var videoModel = new VideoModelDelegate(GetVideoModel);
            args.GlobalScriptObject.Import("ow_video_model", videoModel);

            var formattedDate = new DateFormatDelegate(GetDateFormat);
            args.GlobalScriptObject.Import("ow_date", formattedDate);

            var svgImage = new SVGImageDelegate(GetSVGImage);
            args.GlobalScriptObject.Import("ow_svg_model", svgImage);

            var linkClassess = new LinkClassDelegate(GetLinkClasses);
            args.GlobalScriptObject.Import("ow_get_linkClass", linkClassess);

            var item = new ItemDelegate(GetItem);
            args.GlobalScriptObject.Import("ow_item", item);

            var defaultMedia = new DefaultMediaModelByFieldNameDelegate(GetDefaultMediaModelByField);
            args.GlobalScriptObject.Import("ow_default_media_model_by_field", defaultMedia);

            var defaultDate = new DefaultDateDelegate(GetDefaultDate);
            args.GlobalScriptObject.Import("ow_default_date", defaultDate);

            var formattedEventDate = new EventCardDateDelegate(GetEventCardDate);
            args.GlobalScriptObject.Import("ow_eventcard_date", formattedEventDate);

            var linkFromSettings = new LinkFromSettingsDelegate(GetLinkFromSettings);
            args.GlobalScriptObject.Import("ow_link_from_settings", linkFromSettings);

            var coverImageClass = new CoverImageClassDelegate(GetCoverImageClass);
            args.GlobalScriptObject.Import("ow_cover_image_class", coverImageClass);

            var defaultVideoContent = new DefaultVideoContentDelegate(GetDefaultVideoContent);
            args.GlobalScriptObject.Import("ow_default_video_content", defaultVideoContent);
            var getGatedContent = new GetGatedContentDelegate(GetGatedContent);
            args.GlobalScriptObject.Import("ow_default_gated_content", getGatedContent);


            var descriptionWithoutTag = new DescriptionWithoutTag(GetDescriptionWithoutTag);
            args.GlobalScriptObject.Import("ow_description_Without_Tag", descriptionWithoutTag);
            
            var defaultPageUrl = new DefaultPageUrl(GetDefaultPageUrl);
            args.GlobalScriptObject.Import("ow_default_page_url", defaultPageUrl);

        }

        private LinkModel GetLinkFromSettings(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                return new LinkModel();
            Item settings = SiteExtensions.GetSettingsItem();
            return new LinkModel(settings, fieldName);
        }

        private string GetDefaultDate(Item item, string fieldName, string formatFieldName)
        {
            if (item == null || string.IsNullOrEmpty(formatFieldName) || string.IsNullOrEmpty(fieldName) || string.IsNullOrWhiteSpace(item[fieldName]))
                return string.Empty;
            return DefaultExtensions.GetDefaultDateValue(item, fieldName, formatFieldName);
        }

        private string GetEventCardDate(Item item, string fieldName)
        {
            if (item == null || string.IsNullOrEmpty(fieldName))
                return string.Empty;
            return DefaultExtensions.GetEventCardDateValue(item, fieldName);
        }

        private string GetDefaultVideoContent(string fieldName)
        {
            if ( string.IsNullOrEmpty(fieldName))
                return string.Empty;
            return DefaultExtensions.GetDefaultVideoContent( fieldName);
        }
        private string GetDescriptionWithoutTag(Item item, string fieldName)
        {
            if (item == null  || string.IsNullOrEmpty(fieldName))
                return string.Empty;
            return ItemExtensions.StripTags(item, fieldName);
        }

        private MediaModel GetDefaultMediaModelByField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return new MediaModel();
            return new MediaModel(DefaultExtensions.GetDefaultImageFromSettings(fieldName));
        }

        private static MediaModel GetMediaModel(object input1, object input2)
        {
            if (input1 is Item item && input2 is string fieldName)
            {
                return !string.IsNullOrWhiteSpace(fieldName) ? new MediaModel(item, fieldName) : new MediaModel(item);
            }

            return new MediaModel();
        }

        private static MediaModel GetDefaultMediaModel(object input)
        {
            if (input is Item item && item.IsMediaItem())
            {
                return new MediaModel(item);
            }

            if (input is string mediaUrl && !string.IsNullOrEmpty(mediaUrl))
            {
                return new MediaModel() { Url = mediaUrl };
            }

            return new MediaModel(DefaultExtensions.GetDefaultImage());
        }

        private LinkModel GetLinkModel(object input1, object input2)
        {
            switch (input1)
            {
                case Item item when input2 is string fieldName && !string.IsNullOrWhiteSpace(fieldName):
                    return new LinkModel(item, fieldName);
                case LinkField linkField:
                    return new LinkModel(linkField);
                default:
                    return new LinkModel();
            }
        }

        private static MediaModel GetDefaultMediaModelMobile(object input)
        {
            if (input is Item item && item.IsMediaItem())
            {
                return new MediaModel(item);
            }

            if (input is string mediaUrl && !string.IsNullOrEmpty(mediaUrl))
            {
                return new MediaModel() { Url = mediaUrl };
            }

            return new MediaModel(DefaultExtensions.GetDefaultMobileImage());
        }


        private static VideoModel GetVideoModel(object input)
        {
            if (input is Item item)
            {
                return MediaExtensions.GetVideoModel(item);
            }

            return new VideoModel();
        }

        private string GetGatedContent(Item item)
        {
            return ItemExtensions.GetGatedUrl(item);
        }

        private static MediaModel GetDefaultPlayIcon(object input)
        {
            return new MediaModel(CacheManager.GetOrSet("GetDefaultVideoImage", DefaultExtensions.GetDefaultPlayIcon, 86400));
        }

        private string GetLinkClasses(object input1, object input2, object input3)
        {
            var linkModel = GetLinkModel(input1, input2);
            if (input3 is string customClasses && !string.IsNullOrEmpty(customClasses))
            {
                linkModel.Class += $" {customClasses}";
            }
            return linkModel.Class;
        }
        public static string GetDateFormat(object input, string param1 = null, string param2 = null)
        {
            return DefaultExtensions.GetDefaultDate(input, param1, param2);
        }

        private string GetSVGImage(object input1, object input2, object input3)
        {
            if (input1 is Item item && input2 is string fieldName)
            {
                return string.IsNullOrWhiteSpace(fieldName) ? string.Empty : FieldExtensions.FetchImageAsSVG(item, fieldName, input3?.ToString() ?? string.Empty);
            }
            return string.Empty;
        }
        public Item GetItem(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            return ItemExtensions.GetItem(input);
        }

        private string GetCoverImageClass(Item item)
        {
            return CardExtensions.GetCoverImageClass(item);
        }

        private string GetDefaultPageUrl(string fieldName)
        {
            var settingsItem = SiteExtensions.GetSettingsItem();
            if (settingsItem != null && settingsItem.Fields[fieldName] != null)
            {
                var linkModel = settingsItem.GetLinkFieldModel(fieldName);
                if (linkModel != null)
                {
                    return linkModel.Url;
                }
            }

            return string.Empty;
        }
    }
}