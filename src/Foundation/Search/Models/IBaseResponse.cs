using System.Collections.Generic;
using OneWeb.Foundation.Search.Models.Facets;

namespace OneWeb.Foundation.Search.Models
{
    public interface IBaseResponse<T>
    {
        IList<T> Results { get; set; }
        IList<OneWebFacet> Facets { get; set; }
        ResponseParameters Parameters { get; set; }
    }
}