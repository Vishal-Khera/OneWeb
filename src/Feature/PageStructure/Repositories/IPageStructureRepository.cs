using OneWeb.Feature.PageStructure.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;

namespace OneWeb.Feature.PageStructure.Repositories
{
    public interface IPageStructureRepository : IModelRepository
    {
        SlideWrapperRenderingModel GetSlideWrapper();
        SlideWrapperRenderingModel GetSlideWidget();
    }
}