using OneWeb.Feature.Navigation.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.Navigation.Controllers
{
    public class PageNavigationController : StandardController

    {
        public PageNavigationController(IPageNavigationRepository pageNavigationRepository)
        {
            PageNavigationRepository = pageNavigationRepository;
        }

        protected IPageNavigationRepository PageNavigationRepository { get; }

        protected override object GetModel()
        {
            return PageNavigationRepository.GetModel();
        }
    }
}