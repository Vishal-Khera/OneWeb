using System;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Caching;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Scriban;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Resources.Media;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class MediaExtensions
    {
        public static string GetPrimaryCdnDomain()
        {
            return Sitecore.Configuration.Settings.GetSetting("OneWeb.PrimaryCdnUrl");
        }

        public static string GetSecondaryCdnDomain()
        {
            return Sitecore.Configuration.Settings.GetSetting("OneWeb.SecondaryCdnUrl");
        }

        public static bool IsPrimaryCdnEnabled()
        {
            if (!EnvironmentExtensions.IsContentDeliveryOrStandalone())
                return false;
            var settingsItem = SiteExtensions.GetSettingsItem();
            var primaryCdnEnabled = settingsItem != null && !string.IsNullOrWhiteSpace(GetPrimaryCdnDomain()) &&
                                settingsItem.GetCheckboxStatus(SiteConfigurations.Fields.PrimaryCdnEnabled_FieldName);
            return primaryCdnEnabled == true;
        }

        public static MediaModel GetMediaModel(this Item item, string fieldName)
        {
            return new MediaModel(item, fieldName);
        }

        public static MediaModel GetMediaModel(this Item item, ID fieldId)
        {
            return new MediaModel(item, fieldId);
        }

        public static bool IsSecondaryCdnEnabled()
        {
            if (!EnvironmentExtensions.IsContentDeliveryOrStandalone())
                return false;
            var settingsItem = SiteExtensions.GetSettingsItem();
            var secondaryCdnEnabled = settingsItem != null && !string.IsNullOrWhiteSpace(GetSecondaryCdnDomain()) &&
                                settingsItem.GetCheckboxStatus(SiteConfigurations.Fields.SecondaryCdnEnabled_FieldName);
            return secondaryCdnEnabled == true;
        }
        
        public static string GetMediaUrl(this Item item, bool forceCdn = true)
        {
            if (item == null) return string.Empty;

            if (!item.IsMediaItem()) return item.GetItemUrl();

            var mediaItem = new MediaItem(item);
            var revision = $"revision:{item.Statistics.Revision.ToLowerInvariant()}";

            if (IsPrimaryCdnEnabled() && forceCdn && !string.IsNullOrWhiteSpace(mediaItem.FilePath))
            {
                return $"{CacheManager.GetOrSet("GetPrimaryCdnDomain", GetPrimaryCdnDomain, 86400)}/{mediaItem.FilePath}?{revision}";
            }

            return IsSecondaryCdnEnabled() && forceCdn
                ? $"{CacheManager.GetOrSet("GetSecondaryCdnDomain", GetSecondaryCdnDomain, 86400)}{StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(mediaItem).TrimStart("/sitecore/shell".ToCharArray())).ToLower()}?{revision}"
                : StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(mediaItem).TrimStart("/sitecore/shell".ToCharArray())).ToLower();
        }

        public static string GetMediaUrl(this Item item, string fieldName)
        {
            var mediaItem = item.GetMediaItem(fieldName);
            return GetMediaUrl(mediaItem);
        }

        public static VideoModel GetVideoModel(Item item)
        {
            var videoModel = new VideoModel();
            if (item == null)
                return videoModel;

            if (item.HasVersionInLanguage())
            {
                videoModel.Item = item;
                videoModel.Attributes = item.GetFieldValue(OneWebVideo.Fields.Attributes);
                videoModel.Iframe = new HtmlString(item.GetFieldValue(OneWebVideo.Fields.Iframe));
                videoModel.SitecoreVideo = CheckVideoExtension(item,OneWebVideo.Fields.SitecoreVideo_FieldName);
                videoModel.Id = item.GetFieldValue(OneWebVideo.Fields.Id);
                videoModel.Title = item.GetFieldValue(BaseContent.Fields.Title);
                videoModel.Content = item.GetFieldValue(BaseContent.Fields.Content);
                videoModel.Muted = item.GetCheckboxStatus(OneWebVideo.Fields.Muted_FieldName);
                videoModel.Autoplay = item.GetCheckboxStatus(OneWebVideo.Fields.Autoplay_FieldName);
                videoModel.Loop = item.GetCheckboxStatus(OneWebVideo.Fields.Loop_FieldName);
                videoModel.PlaceholderImage = item.GetMediaModel(OneWebVideo.Fields.PlaceholderImage);

                var videoProvider = item.GetReferencedItem(OneWebVideo.Fields.Provider_FieldName);
                if (videoProvider != null)
                {
                    videoModel.Provider = new VideoProviderModel()
                    {
                        Name = videoProvider.GetFieldValue(OneWebVideoProvider.Fields.Name),
                        Template = videoProvider.GetFieldValue(OneWebVideoProvider.Fields.EmbedTemplate),
                    };
                }

                if (!string.IsNullOrWhiteSpace(videoModel.Iframe.ToHtmlString())) return videoModel;

                var videoVariant = DefaultExtensions.GetDefaultVideoPopupVariant();
                if (videoVariant != null)
                {
                    videoModel.Iframe = MediaExtensions.GetVideoIframe(videoModel);
                }
            }

            return videoModel;
        }

        public static VideoModel GetVideoModel(string videoId)
        {
            var contentRepository = ServiceLocator.ServiceProvider.GetService<IContentRepository>();
            var currentItem = contentRepository.GetItem(ShortID.Parse(videoId).ToID());
            return GetVideoModel(currentItem);
        }

        public static HtmlString GetVideoIframe(VideoModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Provider?.Template))
                return new HtmlString(string.Empty);

            try
            {
                var scribanTemplate = Template.Parse(model.Provider.Template);
                return new HtmlString(scribanTemplate.Render(new
                {
                    id = model.Id,
                    attributes = model.Attributes,
                    muted = model.Muted ? '1' : '0',
                    autoplay = model.Autoplay ? '1' : '0',
                    loop = model.Loop ? '1' : '0',
                    source = !string.IsNullOrEmpty(model.PlaceholderImage.Url) ? "data-src" : "src"
                }));
            }
            catch (Exception ex)
            {
                LogManager.LogError("Error parsing scriban template" + ex.Message, ex);
            }

            return new HtmlString(string.Empty);
        }

        public static MediaModel CheckVideoExtension(Item item, string fieldName)
        {
            var sitecoreVideo = new MediaModel(item, fieldName);
            if (sitecoreVideo.MediaExtension == "mp4" || sitecoreVideo.MediaExtension == "avi" || sitecoreVideo.MediaExtension == "mov" || sitecoreVideo.MediaExtension == "mpg" || sitecoreVideo.MediaExtension == "mkv")
            {
                return sitecoreVideo;
            }
            return new MediaModel();
        }
    }
}