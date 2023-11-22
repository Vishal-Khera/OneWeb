using OneWeb.Feature.PageContent.Models;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.PageContent.Repositories
{
    public interface IComparisonTableRepository : IVariantsRepository
    {
        ComparisonTableModel GetModel(Item item);
    }
}