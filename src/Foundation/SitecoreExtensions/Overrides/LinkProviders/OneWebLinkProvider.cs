using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.XA.Foundation.Multisite.LinkManagers;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.LinkProviders
{
    public class OneWebLinkProvider : LocalizableLinkProvider
    {
        public override string GetItemUrl(Item item, UrlOptions options)
        {
            if (item == null)
                return string.Empty;

            if (item.IsMediaItem())
            {
                return item.GetMediaUrl();
            }

            var itemUrl1 = GetItemUrlOverride(item, options);
            return !string.IsNullOrWhiteSpace(itemUrl1) ? itemUrl1 : base.GetItemUrl(item, options);
        }

        protected string GetItemUrlOverride(Item item, UrlOptions options)
        {
            if (StringExtensions.IdEqualsGuid(item.TemplateID, OneWebVideo.TemplateIdString))
            {
                return $"/oneweb/video/getmodal/{item.ShortId()}";
            }

            return CheckNavigationUrl(item, options);
        }

        private string CheckNavigationUrl(Item currentItem, UrlOptions options)
        {
            if (!string.IsNullOrWhiteSpace(currentItem.Fields[BaseNavigation.Fields.NavigationUrl]?.Value))
            {
                var navigationUrlField = (LinkField)currentItem.Fields[BaseNavigation.Fields.NavigationUrl];
                if (navigationUrlField.IsInternal && !navigationUrlField.TargetID.Equals(currentItem.ID))
                {
                    return new LinkModel(currentItem, BaseNavigation.Fields.NavigationUrl).Url;
                }
            }
            return base.GetItemUrl(currentItem, options);
        }
    }
}