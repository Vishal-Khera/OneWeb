using OneWeb.Feature.HeroBanner.Model;
using Sitecore.Data.Items;

namespace OneWeb.Feature.HeroBanner.Repositories
{
    public interface IHeroBannerRepository
    {
        HeroBannerModel GetHeroBanner(Item item);
    }
}
