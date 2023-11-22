using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.Media.Models
{
    public class VideoRenderingModel : VariantsRenderingModel
    {
        public VideoModel VideoInfo { get; set; }
    }
}