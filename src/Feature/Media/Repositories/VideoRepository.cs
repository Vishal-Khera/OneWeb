using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Mvc.Presentation;
using System.Web;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System.Collections.Generic;
using OneWeb.Feature.Media.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Fields;

namespace OneWeb.Feature.Media.Repositories
{
    public class VideoRepository : VariantsRepository, IVideoRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var videoRenderingModel = new VideoRenderingModel();
            this.FillBaseProperties(videoRenderingModel);
            var datasourceItem = RenderingContext.Current.Rendering.Item;
            if (datasourceItem != null)
            {
                videoRenderingModel.VideoInfo = !StringExtensions.IdEqualsGuid(datasourceItem.TemplateID, OneWebVideo.TemplateIdString) ? new VideoModel() : MediaExtensions.GetVideoModel(datasourceItem);
            }
            return videoRenderingModel;
        }

        public HtmlString GetModal(string videoId)
        {
            var videoModel = MediaExtensions.GetVideoModel(videoId);
            if (videoModel == null)
            {
                return new HtmlString(string.Empty);
            }

            var videoVariant = DefaultExtensions.GetDefaultVideoPopupVariant();
            return videoVariant == null ? new HtmlString(string.Empty) : VariantExtensions.RenderVariant(videoModel.Item, videoVariant, videoModel);
        }
    }
}