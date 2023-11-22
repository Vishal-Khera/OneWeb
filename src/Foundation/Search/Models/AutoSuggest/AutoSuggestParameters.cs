using System.Collections.Generic;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace OneWeb.Foundation.Search.Models.AutoSuggest
{
    public class AutoSuggestParameters<T> : IBaseParameters
    {
        public AutoSuggestParameters()
        {
            QueryOptions = new QueryOptions();
        }
        public AbstractSolrQuery Query { get; set; }
        public string PresetQuery { get; set; }
        public QueryOptions QueryOptions { get; set; }
        public Item CurrentItem { get; set; }
        public string IndexName { get; set; }
        public ISearchIndex Index { get; set; }
        public string Keyword { get; set; }
        public int PageSize { get; set; }
        public int PagesToSkip { get; set; }
        public List<ID> Scope { get; set; }
        public Language Language { get; set; }
    }
}