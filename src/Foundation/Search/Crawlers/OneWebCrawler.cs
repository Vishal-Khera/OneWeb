using OneWeb.Foundation.Serialization;
using Sitecore.ContentSearch;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.Search.Crawlers
{

    public class OneWebItemCrawler : SitecoreItemCrawler
    {
        protected override bool IsExcludedFromIndex(SitecoreIndexableItem
            indexable, bool checkLocation = false)
        {
            var isExcluded = base.IsExcludedFromIndex(indexable, checkLocation);
            if (isExcluded)
                return true;
            Item item = indexable; return (item[BaseSearch.Fields.HideFromAutoSuggest] == "1") ? true : false;
        }
        protected override bool IndexUpdateNeedDelete(SitecoreIndexableItem
            indexable)
        {
            var needDelete = base.IndexUpdateNeedDelete(indexable);
            if (needDelete)
                return true;
            Item item = indexable;
            return (item[BaseSearch.Fields.HideFromAutoSuggest] == "1") ? true :
                false;
        }
    }
}