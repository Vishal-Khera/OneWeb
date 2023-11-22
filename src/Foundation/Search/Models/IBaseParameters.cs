using System.Collections.Generic;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.Models
{
    public interface IBaseParameters
    {
        Item CurrentItem { get; set; }
        List<ID> Scope { get; set; }
        string IndexName { get; set; }
        ISearchIndex Index { get; set; }
        string Keyword { get; set; }
        int PageSize { get; set; }
        int PagesToSkip { get; set; }
    }
}