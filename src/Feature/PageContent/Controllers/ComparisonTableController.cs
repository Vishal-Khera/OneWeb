using OneWeb.Feature.PageContent.Repositories;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class ComparisonTableController : StandardController
    {
        protected IComparisonTableRepository ComparisonRepository { get; }
        public ComparisonTableController(IComparisonTableRepository comparisonRepository)
        {
            ComparisonRepository = comparisonRepository;
        }
        protected override object GetModel()
        {
            var datasource = RenderingContext.Current.Rendering.Item;
            return ComparisonRepository.GetModel(datasource);
        }
    }
}