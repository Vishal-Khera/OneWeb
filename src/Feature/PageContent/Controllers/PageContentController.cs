using OneWeb.Feature.PageContent.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class PageContentController : StandardController
    {
        public PageContentController(IPageContentRepository pageContentRepository)
        {
            _pageContentRepository = pageContentRepository;
        }

        protected IPageContentRepository _pageContentRepository { get; }


        protected override object GetModel()
        {
            return _pageContentRepository.GetModel();
        }
    }
}