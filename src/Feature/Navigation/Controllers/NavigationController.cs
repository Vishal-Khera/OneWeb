using OneWeb.Feature.Navigation.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.Navigation.Controllers
{
    public class NavigationController : StandardController

    {
        public NavigationController(INavigationRepository navigationRepository)
        {
            NavigationRepository = navigationRepository;
        }

        protected INavigationRepository NavigationRepository { get; }

        protected override object GetModel()
        {
            return NavigationRepository.GetModel();
        }
    }
}