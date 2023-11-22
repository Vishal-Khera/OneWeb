using OneWeb.Feature.HeroBanner.Repositories;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.RenderingVariants.Controllers;
using System.Web.Mvc;

namespace OneWeb.Feature.HeroBanner.Controllers
{
    public class HeroBannerController : VariantsController
    {
        private IHeroBannerRepository _heroBannerRepository;

        public HeroBannerController(IHeroBannerRepository heroBannerRepository)
        {
            _heroBannerRepository = heroBannerRepository;
        }
        
        protected override object GetModel()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item;
            if(datasourceItem == null)
            {
                return new EmptyResult();
            }

            var heroBannerModel=_heroBannerRepository.GetHeroBanner(datasourceItem);
            return heroBannerModel;
        }
    }
}