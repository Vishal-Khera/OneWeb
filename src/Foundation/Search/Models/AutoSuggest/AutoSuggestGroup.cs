using System.Collections.Generic;

namespace OneWeb.Foundation.Search.Models.AutoSuggest
{
    public class AutoSuggestGroup<T>
    {
        public string GroupIdentifier { get; set; }
        public string GroupLimit { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public int Count { get; set; }
        public List<T> Results { get; set; }
    }
}
