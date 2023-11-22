using System.Web.Mvc;
using OneWeb.Feature.Navigation.Repositories;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.Navigation.Controllers
{
    public class LanguageSwitcherController : StandardController

    {
        public LanguageSwitcherController(ILanguageSwitcherRepository languageSwitcherRepository)
        {
            _languageSwitcherRepository = languageSwitcherRepository;
        }

        protected ILanguageSwitcherRepository _languageSwitcherRepository { get; }

        // GET: LanguageSwitcher
        public ActionResult OneWebLanguageSwitcher()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item;
            if (datasourceItem != null && StringExtensions.IdEqualsGuid(datasourceItem.TemplateID,
                    Foundation.Serialization.OneWebLanguageSwitcher.TemplateIdString))
                return View("~/Views/Feature/LanguageSwitcher/OneWebLanguageSwitcher.cshtml",
                    _languageSwitcherRepository.GetSiteLanguages(datasourceItem));

            return new EmptyResult();
        }
    }
}