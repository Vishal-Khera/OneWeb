using OneWeb.Feature.PageContent.Repositories;
using OneWeb.Foundation.Search.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class GalleryController : StandardController
    {
        public GalleryController(IGalleryRepository galleryRepository)
        {
            GalleryRepository = galleryRepository;
        }
        protected IGalleryRepository GalleryRepository { get; }
        public ActionResult RenderGallery()
        {
            var galleryList = GalleryRepository.GetGalleryList();
            return View(galleryList);
        }
    }
}