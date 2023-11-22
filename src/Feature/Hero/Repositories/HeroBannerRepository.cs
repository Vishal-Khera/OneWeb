using OneWeb.Feature.HeroBanner.Model;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.HeroBanner.Repositories
{
    public class HeroBannerRepository : VariantsRepository, IHeroBannerRepository
    {
        protected IVariantsRepository _variantsRespository;

        public HeroBannerRepository(IVariantsRepository variantsRespository)
        {
            this._variantsRespository = variantsRespository;
        }

        public HeroBannerModel GetHeroBanner(Item item)
        {
            var heroBannerModel = new HeroBannerModel
            {
                TemplateName = item.TemplateName,
                DatasourceItem = item
            };
            FillBaseProperties(heroBannerModel);
            return heroBannerModel;
        }
    }
}