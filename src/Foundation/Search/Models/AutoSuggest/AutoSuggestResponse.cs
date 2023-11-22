using System.Collections.Generic;
using System.Collections.Specialized;

namespace OneWeb.Foundation.Search.Models.AutoSuggest
{
    public class AutoSuggestResponse<T>
    {
        public string Keyword { get; set; }
        public List<AutoSuggestGroup<T>> Groups { get; set; }
        public Dictionary<string, string> SuggestedSearches { get; set; }

        public AutoSuggestResponse()
        {
            Groups = new List<AutoSuggestGroup<T>>();
            SuggestedSearches = new Dictionary<string, string>();
        }
    }
}