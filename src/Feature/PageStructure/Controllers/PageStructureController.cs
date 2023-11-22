using System.Web.Mvc;
using OneWeb.Feature.PageStructure.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.PageStructure.Controllers
{
    public class PageStructureController : StandardController
    {
        public PageStructureController(IPageStructureRepository pageStructureRepository)
        {
            PageStructureRepository = pageStructureRepository;
        }

        protected IPageStructureRepository PageStructureRepository { get; }

        protected override object GetModel()
        {
            return PageStructureRepository.GetModel();
        }

        public ActionResult GetSlideWrapper()
        {
            return View("~/Views/Feature/PageStructure/OneWebSlideWrapper.cshtml",PageStructureRepository.GetSlideWrapper());
        }

        public ActionResult GetSlideWidget()
        {
            return View("~/Views/Feature/PageStructure/OneWebSlideWidget.cshtml", PageStructureRepository.GetSlideWidget());
        }
    }
}