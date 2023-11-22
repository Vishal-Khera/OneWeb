using OneWeb.Foundation.Search.Extensions;
using OneWeb.Foundation.Search.Models;
using OneWeb.Foundation.Search.Models.AutoSuggest;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Models.SolrNetSearch;
using OneWeb.Foundation.Search.Service;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace OneWeb.Foundation.Search.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly SearchService _searchService;

        public SearchRepository(SearchService searchService)
        {
            _searchService = searchService;
        }

        public IBaseResponse<T> ContentSearch<T>(ContentParameters<T> parameters) where T : ContentModel
        {
            SearchExtensions.ParseContextIndex(parameters);
            if (string.IsNullOrWhiteSpace(parameters.IndexName)) return new ContentResponse<T>();

            SearchExtensions.ParseParameters(parameters);
            SearchExtensions.ContentParseParameters(parameters);
            SearchExtensions.ContentExecuteParameters(parameters);
            return _searchService.InitiateSearch(parameters);
        }

        public AutoSuggestResponse<T> AutoSuggestSearch<T>(AutoSuggestParameters<T> parameters) where T : AutoSuggestModel
        {
            SearchExtensions.ParseContextIndex(parameters);
            if (string.IsNullOrWhiteSpace(parameters.IndexName)) return new AutoSuggestResponse<T>();
            SearchExtensions.ParseParameters(parameters);
            SearchExtensions.AutoSuggestCreatePredicate(parameters);
            return _searchService.InitiateSearch(parameters) as AutoSuggestResponse<T>;
        }

        public IBaseResponse<T> SolrNetSearch<T>(SolrNetParameters<T> parameters) where T : SolrNetModel
        {
            SearchExtensions.ParseContextIndex(parameters);
            if (string.IsNullOrWhiteSpace(parameters.IndexName)) return new ContentResponse<T>();

            SearchExtensions.ParseParameters(parameters);
            SearchExtensions.SolrNetParseParameters(parameters);
            
            SolrNetResponse<T> retObj = _searchService.InitiateSearch(parameters);
            return retObj;
        }
    }
}