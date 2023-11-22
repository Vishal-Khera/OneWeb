using Sitecore;
using Sitecore.Pipelines.HttpRequest;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web;
using OneWeb.Foundation.Login.Repositories;
using System.Web;
using System;
using Sitecore.Security.Authentication;

namespace OneWeb.Foundation.Login.Pipelines
{
    public class GatedContent : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (Context.Item == null || !args.Url.ItemPath.Contains("/sitecore/content"))
                return;
            if (string.IsNullOrEmpty(args.Url.ItemPath))
                return;
            var currentPage = ItemExtensions.GetItem(args.Url.ItemPath);
            if (currentPage == null)
                return;
            bool isGatedContent = currentPage[BaseLogin.Fields.IsGatedContent_FieldName] == "1";
            if (isGatedContent)
            {
                UserService userService = new UserService();
                if (userService.IsLoggedInCookieUser())
                {
                    return;
                }
                Item settings = SiteExtensions.GetSettingsItem();
                var loginPage = settings.GetReferencedItem(SiteConfigurations.Fields.GatedLoginPage_FieldName);
                if (loginPage != null)
                {
                    Context.Item = loginPage;
                    Context.Items["custom::ItemResolved"] = true;
                    WebUtil.Redirect(loginPage.GetItemUrl() + "?redirectUrl=" + args.LocalPath);
                }
            }
        }
    }
}