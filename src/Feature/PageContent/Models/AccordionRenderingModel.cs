using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.DynamicPlaceholders.Models;
using System.Collections.Generic;
using System.Linq;

namespace OneWeb.Feature.PageContent.Models
{
    public class AccordionRenderingModel : CompositeComponentRenderingModel
    {
        public string JsonDataProperties { get; set; }
    }
}

string keyword = "Sitecore";
ISearchIndex searchIndex = ContentSearchManager.GetIndex("your_index_name");
using (IProviderSearchContext context = searchIndex.CreateSearchContext())
{
    var query = context.GetQueryable<SearchResultItem>()
                       .Where(item => item.TemplateName == "Article" && item.Title.Contains(keyword))
                       .OrderByDescending(item => item.CreatedDate);
    var results = query.GetResults();
    foreach (var result in results.Hits)
    {
        // Process and display search results
    }
}
string keyword = "Sitecore"; 
ISearchIndex searchIndex = ContentSearchManager.GetIndex("your_index_name"); 
using (IProviderSearchContext context = searchIndex.CreateSearchContext()) 
{
    var query = context.GetQueryable<SearchResultItem>()
                   .Where(item => item.TemplateName == "Product" && item.Content.Contains(keyword));
    var facets = query.FacetOn(item => item.Category); 
    var results = query.GetResults(); // Process search results and facets
}
string keyword = "Sitecore";
ISearchIndex searchIndex = ContentSearchManager.GetIndex("your_index_name");
using (IProviderSearchContext context = searchIndex.CreateSearchContext())
{
    var query = context.GetQueryable<SearchResultItem>();

    var boostedResults = query.OrderByDescending(item => item.Title.Contains(keyword) ? 10 : 0)
                             .ThenByDescending(item => item.Content.Contains(keyword) ? 5 : 0);

    var results = boostedResults.GetResults(); // Process and display search results
}

using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.SolrNetIntegration;

string keyword = "Sitecore";
string indexName = "your_index_name";

// Get the Solr search index
var searchIndex = ContentSearchManager.GetIndex(indexName);

using (var context = searchIndex.CreateSearchContext())
{
    // Create a Solr queryable
    var queryable = context.GetQueryable<SearchResultItem>();

    // Create the search query using LINQ expressions
    var query = queryable.Where(item => item.Content.Contains(keyword));

    // Execute the search query
    var results = query.GetResults();

    // Process and display search results
    foreach (var result in results.Hits)
    {
        // Access search result properties using result.Document
        var title = result.Document.Title;
        var content = result.Document.Content;
        // Process and display other properties as needed
    }
}
