using OneWeb.Foundation.Search.Models;
using OneWeb.Foundation.Search.Models.AutoSuggest;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Models.SolrNetSearch;

namespace OneWeb.Foundation.Search.Repositories
{
    public interface ISearchRepository
    {
        IBaseResponse<T> ContentSearch<T>(ContentParameters<T> parameters)
            where T : ContentModel;
        IBaseResponse<T> SolrNetSearch<T>(SolrNetParameters<T> parameters)
            where T : SolrNetModel;

        AutoSuggestResponse<T> AutoSuggestSearch<T>(AutoSuggestParameters<T> parameters)
            where T : AutoSuggestModel;
    }
}