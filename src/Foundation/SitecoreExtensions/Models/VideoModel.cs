using System.Web;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.SitecoreExtensions.Models
{
    public class VideoModel
    {
        public Item Item { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Muted { get; set; }
        public bool Autoplay { get; set; }
        public bool Loop { get; set; }
        public HtmlString Iframe { get; set; }
        public MediaModel SitecoreVideo { get; set; }
        public string Attributes { get; set; }
        public MediaModel PlaceholderImage { get; set; }
        public VideoProviderModel Provider { get; set; }

        public VideoModel()
        {
            Provider = new VideoProviderModel();
        }
    }
}
