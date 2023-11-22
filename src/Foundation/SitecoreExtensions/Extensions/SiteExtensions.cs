using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Caching;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Sites;
using Site = OneWeb.Foundation.Serialization.Site;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class SiteExtensions
    {
        private static string[] noPreloadfBrowsers = { "InternetExplorer", "Firefox" };
        public static Item GetSiteRoot(Item item = null)
        {
            Item siteRoot = null;
            if (item != null)
            {
                siteRoot = item.Axes.GetAncestors().FirstOrDefault(x => StringExtensions.IdEqualsGuid(x.TemplateID, Site.TemplateIdString)) ??
                            item.Database.GetItem(GetSiteRootByItem(item));
            }

            if (siteRoot != null) return siteRoot;

            if (Context.Site != null && Context.Site.Name.ToLower() != "shell")
            {
                siteRoot = ItemExtensions.GetItem(Context.Site.RootPath);
            }

            return siteRoot;
        }

        public static Item GetHomeItem(Item item = null)
        {
            if (item != null)
            {
                return item.Axes.GetAncestors().FirstOrDefault(x => StringExtensions.IdEqualsGuid(x.TemplateID, Home.TemplateIdString));
            }

            if (Context.Site != null)
            {
                return ItemExtensions.GetItem(Context.Site.StartPath);
            }

            return null;
        }

        public static Item GetSettingsItem(Item item = null)
        {
            return GetSiteRoot(item)?.Children
                .FirstOrDefault(x => StringExtensions.IdEqualsGuid(x.TemplateID, Settings.TemplateIdString));
        }

        public static Item GetTenantItem(Item item = null)
        {
            return GetSiteRoot(item).Axes
                .GetAncestors().FirstOrDefault(x => StringExtensions.IdEqualsGuid(x.TemplateID, Tenant.TemplateIdString));
        }

        public static bool IsOneWebSite(SiteContext site = null)
        {
            if (site == null)
                site = Context.Site;
            return site.RootPath.ToLower().Contains("oneweb");
        }

        public static string GetLanguage(string siteLanguage, string contextLanguge)
        {
            return string.IsNullOrWhiteSpace(siteLanguage) ? contextLanguge : siteLanguage;
        }
        public static string GetSiteRootByItem(Item contextItem)
        {
            if (contextItem.Paths.IsContentItem)
            {
                return contextItem.Axes.GetAncestors()
                    .FirstOrDefault(x => x.TemplateID.Equals(Site.TemplateId))?.Paths.FullPath;
            }

            if (!contextItem.Paths.IsMediaItem) return "/sitecore/content";

            var rootPath =
                contextItem.Paths.FullPath.Replace("/sitecore/media library/Project/OneWeb/OneWeb/",
                    string.Empty);
            if (rootPath.Contains("/"))
            {
                rootPath = rootPath.Substring(0, rootPath.IndexOf("/", StringComparison.Ordinal));
            }

            return $"/sitecore/content/OneWeb/OneWeb/{rootPath}";
        }

        public static string TransformStyleLinks(string inputLink)
        {
            if (!Context.PageMode.IsNormal || string.IsNullOrWhiteSpace(inputLink)
                || noPreloadfBrowsers.Contains(Context.HttpContext.Request.Browser.Browser)) return inputLink;

            if (MediaExtensions.IsSecondaryCdnEnabled())
            {
                inputLink = inputLink.Replace("href=\"", $"href=\"{MediaExtensions.GetSecondaryCdnDomain()}");
            }

            return inputLink + Environment.NewLine;
        }

        public static string TransformScriptLinks(string inputLink)
        {
            if (!Context.PageMode.IsNormal || string.IsNullOrWhiteSpace(inputLink)) return inputLink;

            inputLink = inputLink.Replace("/>", " defer />" + Environment.NewLine);
            inputLink = inputLink.Replace("></script>", " defer /></script>" + Environment.NewLine);
            if (MediaExtensions.IsSecondaryCdnEnabled())
            {
                inputLink = inputLink.Replace("src=\"", $"src=\"{MediaExtensions.GetSecondaryCdnDomain()}");
            }
            return inputLink;
        }


        public static string PreConnectExternalServers()
        {
            if (!Context.PageMode.IsNormal)
                return string.Empty;

            var urlList = new List<string>();
            var stringBuilder = new StringBuilder();
            var settingsItem = GetSettingsItem();
            var externalAssets = new List<string>();
            if (!string.IsNullOrWhiteSpace(
                    settingsItem.GetFieldValue(SiteConfigurations.Fields.ExternalStyles_FieldName)))
            {
                foreach (var externalAsset in settingsItem.GetFieldValue(SiteConfigurations.Fields.ExternalStyles_FieldName).Split('\n'))
                {
                    try
                    {
                        Match match = Regex.Match(externalAsset, "href=\"?(.*?)\"");
                        externalAssets.Add(match.Groups[1].Value);
                    }
                    catch (Exception e)
                    {
                        urlList.Add("Invalid Style in External Styles");
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(
                    settingsItem.GetFieldValue(SiteConfigurations.Fields.ExternalScripts_FieldName)))
            {
                externalAssets.AddRange(settingsItem.GetFieldValue(SiteConfigurations.Fields.ExternalScripts_FieldName).Split('\n'));
            }
            if (!string.IsNullOrWhiteSpace(
                    settingsItem.GetFieldValue(SiteConfigurations.Fields.ExternalLinks_FieldName)))
            {
                foreach (var externalLink in settingsItem.GetFieldValue(SiteConfigurations.Fields.ExternalLinks_FieldName).Split('\n'))
                {
                    try
                    {
                        Match match = Regex.Match(externalLink, "src=\"?(.*?)\"");
                        externalAssets.Add(match.Groups[1].Value);
                    }
                    catch (Exception e)
                    {
                        urlList.Add("Invalid String in External Links");
                    }
                }
            }

            if (!externalAssets.Any())
            {
                return stringBuilder.ToString();
            }

            try
            {
                foreach (var assetUri in externalAssets.Select(x => new Uri(x)).Distinct())
                {
                    var currentUrl = $"{assetUri.Scheme}://{assetUri.Host}";
                    urlList.Add(currentUrl);
                }
            }
            catch (Exception e)
            {
                stringBuilder.AppendLine("Invalid String in External Styles/Scripts");
            }

            foreach (var url in urlList.Distinct())
            {
                stringBuilder.AppendLine(
                    $"<link rel=\"preconnect\" href=\"{url}\">");
                stringBuilder.AppendLine(
                    $"<link rel=\"dns-prefetch\" href=\"{url}\">");
            }
            return stringBuilder.ToString();
        }

        public static string LoadFontResources()
        {
            var stringBuilder = new StringBuilder();
            var settingsItem = GetSettingsItem();
            var fontAssets = new List<Item>();
            if (!string.IsNullOrWhiteSpace(
                    settingsItem.GetFieldValue(SiteConfigurations.Fields.Fonts_FieldName)))
            {
                fontAssets.AddRange(ItemExtensions.GetTargetItems(settingsItem, SiteConfigurations.Fields.Fonts_FieldName));
            }

            foreach (var fontUrl in fontAssets.Select(font => font.GetMediaUrl(true)).Where(fontUrl => !string.IsNullOrWhiteSpace(fontUrl)))
            {
                stringBuilder.AppendLine(
                    $"<link rel=\"preload\" href=\"{fontUrl}\" as=\"font\" type=\"font/woff2\" crossorigin=\"anonymous\">");
            }

            return stringBuilder.ToString();
        }

        public static string LoadExternalStyles()
        {
            var stringBuilder = new StringBuilder();
            var settingsItem = GetSettingsItem();
            var externalStyles =
                settingsItem.GetFieldValue(SiteConfigurations.Fields.ExternalStyles_FieldName);

            return externalStyles;
        }

        public static string LoadExternalScripts()
        {
            var stringBuilder = new StringBuilder();
            var settingsItem = GetSettingsItem();
            var externalScripts =
                settingsItem.GetFieldValue(SiteConfigurations.Fields.ExternalScripts_FieldName);

            return externalScripts;
        }

        public static string LoadExternalLinks()
        {
            var settingsItem = GetSettingsItem();
            var externalLinks =
                settingsItem.GetFieldValue(SiteConfigurations.Fields.ExternalLinks_FieldName);

            return externalLinks;
        }
        public static string GetIndex()
        {
            string Index = null;
            Sitecore.Data.Database db = Sitecore.Context.Database;
            if (db.Name == "master")
            {
                Index = GetSiteRoot().Name + "_ow_cms";
                return Index;
            }
            Index = GetSiteRoot().Name + "_ow";
            return Index;
        }

        public static string LoadFormScripts()
        {
            var domain = string.Empty;

            if (MediaExtensions.IsSecondaryCdnEnabled())
            {
                domain = MediaExtensions.GetSecondaryCdnDomain();
            }
            var resourceList = new[]
            {
                $"<script src=\"{domain}/sitecore%20modules/Web/ExperienceForms/scripts/jquery-2.1.3.min.js\"></script>",
                $"<script src=\"{domain}/sitecore%20modules/Web/ExperienceForms/scripts/jquery.validate.min.js\"></script>",
                $"<script src=\"{domain}/sitecore%20modules/Web/ExperienceForms/scripts/jquery.validate.unobtrusive.min.js\"></script>",
                $"<script src=\"{domain}/sitecore%20modules/Web/ExperienceForms/scripts/jquery.unobtrusive-ajax.min.js\"></script>",
                $"<script src=\"{domain}/sitecore%20modules/Web/ExperienceForms/scripts/form.validate.js\"></script>",
                $"<script src=\"{domain}/sitecore%20modules/Web/ExperienceForms/scripts/form.tracking.js\"></script>",
                $"<script src=\"{domain}/sitecore%20modules/Web/ExperienceForms/scripts/form.conditions.js\"></script>",
                $"<script src=\"{domain}/sitecore%20modules/Web/ExperienceForms/scripts/formsextensions.validate.js\"></script>"
            };
            return string.Join(Environment.NewLine, resourceList);
        }

        public static string GetContentSecurityPolicy()
        {
            var contentSecurityPolicy = string.Empty;
            var settingsItem = GetSettingsItem();
            if (settingsItem != null)
            {
                contentSecurityPolicy = settingsItem.GetFieldValue(SiteConfigurations.Fields.ContentSecurityPolicy);
            }

            return contentSecurityPolicy;
        }

        public static string GetAccessControlAllowedOrigin()
        {
            var accessControlAllowedOrigin = string.Empty;
            var settingsItem = GetSettingsItem();
            if (settingsItem != null)
            {
                accessControlAllowedOrigin = settingsItem.GetFieldValue(SiteConfigurations.Fields.AccessControlAllowOrigin);
            }

            return accessControlAllowedOrigin;
        }

        public static string GetXFrameOptions()
        {
            var xFrameOptions = string.Empty;
            var settingsItem = GetSettingsItem();
            if (settingsItem != null)
            {
                xFrameOptions = settingsItem.GetFieldValue(SiteConfigurations.Fields.XFrameOptions);
            }

            return xFrameOptions;
        }
    }
}