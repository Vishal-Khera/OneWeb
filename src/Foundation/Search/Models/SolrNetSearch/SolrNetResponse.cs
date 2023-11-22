using System.Collections.Generic;
using OneWeb.Foundation.Search.Models.Facets;
using Sitecore.Publishing.Pipelines.PublishItem;
using SolrNet;

namespace OneWeb.Foundation.Search.Models.SolrNetSearch
{
    public class SolrNetResponse<T> :  IBaseResponse<T>
    {
        public IList<OneWebFacet> Facets { get; set; }
        public ResponseParameters Parameters { get; set; }
        public IList<T> Results { get; set; }
    }
}