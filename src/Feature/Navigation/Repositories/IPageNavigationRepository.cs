using OneWeb.Feature.Navigation.Models;

namespace OneWeb.Feature.Navigation.Repositories
{
    public interface IPageNavigationRepository
    {
        PageNavigationRenderingModel GetModel();
    }
}