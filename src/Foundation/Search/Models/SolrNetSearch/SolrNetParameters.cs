using Sitecore.Data;
using Sitecore.Data.Items;
using SolrNet;
using SolrNet.Commands.Parameters;
using System.Collections.Generic;
using Sitecore.ContentSearch;
using Sitecore.Globalization;

namespace OneWeb.Foundation.Search.Models.SolrNetSearch
{
    public class SolrNetParameters<T> : IBaseParameters
    {
        public SolrNetParameters()
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