using System;
using System.Collections.Generic;
using OneWeb.Foundation.Search.Models.Facets;
using Sitecore.ContentSearch.Linq;

namespace OneWeb.Foundation.Search.Models.ContentSearch
{
    [Serializable]
    public class ContentResponse<T> : IBaseResponse<T>
    {
        public IList<OneWebFacet> Facets { get; set; }
        public ResponseParameters Parameters { get; set; }
        public IList<T> Results { get; set; }
    }
}