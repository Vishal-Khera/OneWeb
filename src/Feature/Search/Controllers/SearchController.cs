using System.Web.Helpers;
using System.Web.Mvc;
using OneWeb.Feature.Search.Models;
using OneWeb.Feature.Search.Repositories;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Mvc.Controllers;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;

namespace OneWeb.Feature.Search.Controllers
{
    public class SearchController : StandardController
    {
        public SearchController(ISearchRepository searchRepository, IContentRepository contentRepository ,Foundation.Search.Repositories.ISearchRepository foundationSearchRepository)
        {
            SearchRepository = searchRepository;
            FoundationSearchRepository = foundationSearchRepository;
            ContentRepository = contentRepository;
        }

        protected ISearchRepository SearchRepository { get; }
        protected IContentRepository ContentRepository { get; }
        protected Foundation.Search.Repositories.ISearchRepository FoundationSearchRepository { get; }
        public ActionResult RenderSearchBox()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item;
            if (datasourceItem != null)
            {
                return View("~/Views/Feature/Search/OneWebSearchBox.cshtml", SearchRepository.RenderSearchBox(datasourceItem));
            }

            return new EmptyResult();
        }
        public ActionResult RenderSearchTemplate()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item;
            if (datasourceItem != null)
            {
                return View("~/Views/Feature/Search/OneWebSearchTemplate.cshtml",SearchRepository.RenderSearchTemplate(datasourceItem));
            }

            return new EmptyResult();
        }

        public ActionResult RenderPagination()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item;
            if (datasourceItem != null)
            {
                return View("~/Views/Feature/Search/OneWebPagination.cshtml", SearchRepository.RenderPagination(datasourceItem));
            }

            return new EmptyResult();
        }

        public JsonResult GetFilter(AjaxSearchParameters parameters)
        {
            if (parameters == null || string.IsNullOrWhiteSpace(parameters.SourceId))
                return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            parameters.SourceItem = ContentRepository.GetItem(ShortID.Parse(parameters.SourceId).ToID());
            if (parameters.SourceItem != null &&
                Foundation.SitecoreExtensions.Extensions.StringExtensions.IdEqualsGuid(parameters.SourceItem.TemplateID,
                    OneWebSearchFilter.TemplateIdString))
            {
                using (new Sitecore.Globalization.LanguageSwitcher(parameters.Language = SiteExtensions.GetLanguage(parameters.Language, Sitecore.Context.Language.Name)))
                {
                    return SearchRepository.GetFilter(parameters.SourceItem);
                }
            }

            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetSearchResults(AjaxSearchParameters parameters)
        {
            if (parameters == null || string.IsNullOrWhiteSpace(parameters.SourceId))
                return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            parameters.SourceItem = ContentRepository.GetItem(ShortID.Parse(parameters.SourceId).ToID());
            if (parameters.SourceItem != null)
            {
                using (new Sitecore.Globalization.LanguageSwitcher(parameters.Language = SiteExtensions.GetLanguage(parameters.Language, Sitecore.Context.Language.Name)))
                {
                    return SearchRepository.GetSearchResults(FoundationSearchRepository, parameters);
                }
            }

            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAutoSuggestResults(AjaxSearchParameters parameters)
        {
            if (parameters == null || string.IsNullOrWhiteSpace(parameters.SourceId))
                return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            parameters.SourceItem = ContentRepository.GetItem(ShortID.Parse(parameters.SourceId).ToID());
            if (parameters.SourceItem != null)
            {
                if (parameters.SourceItem.GetCheckboxStatus(BaseSearchConfiguration.Fields
                        .ShowResultsByDefault_FieldName) && string.IsNullOrWhiteSpace(parameters.Keyword))
                {
                    parameters.Keyword = "*";
                }

                if (!string.IsNullOrWhiteSpace(parameters.Keyword))
                {
                    using (new Sitecore.Globalization.LanguageSwitcher(parameters.Language = SiteExtensions.GetLanguage(parameters.Language, Sitecore.Context.Language.Name)))
                    {
                        return SearchRepository.GetAutoSuggestResults(FoundationSearchRepository, parameters);
                    }
                }
            }

            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}