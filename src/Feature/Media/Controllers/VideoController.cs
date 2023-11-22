using System.Web;
using OneWeb.Feature.Media.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.Media.Controllers
{
    public class VideoController : StandardController
    {

        public VideoController(IVideoRepository videoRepository)
        {
            VideoRepository = videoRepository;
        }
        protected IVideoRepository VideoRepository { get; }

        public HtmlString GetModal(string videoId)
        {
            return VideoRepository.GetModal(videoId);
        }

        protected override object GetModel()
        {
            return VideoRepository.GetModel();
        }
    }
}