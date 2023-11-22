using OneWeb.Feature.PageContent.Models;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.PageContent.Repositories
{
    public interface IBusinessRepository : IVariantsRepository
    {
        object GetData(BusinessRequest request);

        BusinessRenderingModel GetModel(BusinessRequest request);
    }
}