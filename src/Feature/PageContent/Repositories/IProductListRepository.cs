using OneWeb.Feature.PageContent.Models;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using Sitecore.Data.Items;

namespace OneWeb.Feature.PageContent.Repositories
{
    public interface IProductListRepository:IVariantsRepository
    {
        ProductListRenderingModel GetModel(Item item);
    }
}