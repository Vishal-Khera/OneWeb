using System.Web;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.Media.Repositories
{
    public interface IVideoRepository : IVariantsRepository
    {
        HtmlString GetModal(string videoId);
    }
}