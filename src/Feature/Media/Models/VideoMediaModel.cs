using OneWeb.Foundation.SitecoreExtensions.Models;
using System.Web;

namespace OneWeb.Feature.Media.Models
{
    public class VideoMediaModel
    {
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        public MediaModel VideoPlaceholder { get; set; }
        public HtmlString Iframe { get; set; }
    }
}