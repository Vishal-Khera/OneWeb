using System.Web.Mvc;
using OneWeb.Feature.Search.Models;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;

namespace OneWeb.Feature.Search.Repositories
{
    public interface ISearchRepository : IModelRepository
    {
        SearchBoxRenderingModel RenderSearchBox(Item datasourceItem);
        HandleBarRenderingModel RenderSearchTemplate(Item datasourceItem);
        PaginationRenderingModel RenderPagination(Item datasourceItem);
        JsonResult GetFilter(Item datasourceItem);
        JsonResult GetSearchResults(Foundation.Search.Repositories.ISearchRepository repository, AjaxSearchParameters parameters);
        JsonResult GetAutoSuggestResults(Foundation.Search.Repositories.ISearchRepository repository, AjaxSearchParameters parameters);
    }
}