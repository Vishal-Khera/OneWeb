using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Microsoft.Extensions.DependencyInjection;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.XA.Feature.Redirects.Pipelines.HttpRequest;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.Multisite;
using Sitecore.XA.Foundation.Multisite.Extensions;
using Sitecore.XA.Foundation.SitecoreExtensions.Comparers;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines
{
    public class RedirectMapResolver : HttpRequestProcessor
    {
        private string ResolvedMappingsPrefix => "SXA-Redirect-ResolvedMappings-" + this.Context.Database.Name + "-" + this.Context.Site.Name;

        private string AllMappingsPrefix => "SXA-Redirect-AllMappings-" + this.Context.Database.Name + "-" + this.Context.Site.Name;

        public int CacheExpiration { set; get; }

        protected IContext Context { get; } = ServiceLocator.ServiceProvider.GetService<IContext>();

        public override void Process(HttpRequestArgs args)
        {
            if (this.Context.Item != null || this.Context.Database == null || !this.Context.Site.IsSxaSite() || this.IsFile(this.Context.Request.FilePath))
                return;
            string str = this.EnsureSlashes(this.Context.Request.FilePath.ToLowerInvariant());
            RedirectMapping mapping = this.GetResolvedMapping(str);
            bool flag = mapping != null;
            if (mapping == null)
                mapping = this.FindMapping(str);
            if (mapping != null && !flag)
            {
                if (!(HttpRuntime.Cache[this.ResolvedMappingsPrefix] is Dictionary<string, RedirectMapping> dictionary1))
                    dictionary1 = new Dictionary<string, RedirectMapping>();
                Dictionary<string, RedirectMapping> dictionary2 = dictionary1;
                dictionary2[str] = mapping;
                HttpRuntime.Cache.Add(this.ResolvedMappingsPrefix, (object)dictionary2, (CacheDependency)null, DateTime.UtcNow.AddMinutes((double)this.CacheExpiration), TimeSpan.Zero, CacheItemPriority.Normal, (CacheItemRemovedCallback)null);
            }
            if (mapping == null || HttpContext.Current == null)
                return;
            string targetUrl = this.GetTargetUrl(mapping, str);
            if (mapping.RedirectType == RedirectType.Redirect301)
                this.Redirect301(HttpContext.Current.Response, targetUrl);
            if (mapping.RedirectType == RedirectType.Redirect302)
                HttpContext.Current.Response.Redirect(targetUrl, true);
            if (mapping.RedirectType != RedirectType.ServerTransfer)
                return;
            HttpContext.Current.Server.TransferRequest(targetUrl);
        }

        protected virtual bool IsFile(string filePath) => string.IsNullOrEmpty(filePath) || WebUtil.IsExternalUrl(filePath) || File.Exists(HttpContext.Current.Server.MapPath(filePath));

        protected virtual RedirectMapping GetResolvedMapping(string filePath) => HttpRuntime.Cache[this.ResolvedMappingsPrefix] is Dictionary<string, RedirectMapping> dictionary && dictionary.ContainsKey(filePath) ? dictionary[filePath] : (RedirectMapping)null;

        protected virtual RedirectMapping FindMapping(string filePath)
        {
            foreach (RedirectMapping mappings in (IEnumerable<RedirectMapping>)this.MappingsMap)
            {
                if (!mappings.IsRegex && mappings.Pattern == filePath || mappings.IsRegex && mappings.Regex.IsMatch(filePath))
                    return mappings;
            }
            return (RedirectMapping)null;
        }

        protected virtual IList<RedirectMapping> MappingsMap
        {
            get
            {
                if (!(HttpRuntime.Cache[this.AllMappingsPrefix] is List<RedirectMapping> mappingsMap))
                {
                    mappingsMap = new List<RedirectMapping>();
                    Item settingsItem = ServiceLocator.ServiceProvider.GetService<IMultisiteContext>().GetSettingsItem(this.Context.Database.GetItem(this.Context.Site.StartPath));
                    Item obj1 = settingsItem != null ? settingsItem.FirstChildInheritingFrom(Sitecore.XA.Feature.Redirects.Templates.RedirectMapGrouping.ID) : (Item)null;
                    if (obj1 != null)
                    {
                        Item[] array = ((IEnumerable<Item>)obj1.Axes.GetDescendants()).Where<Item>((Func<Item, bool>)(i => i.InheritsFrom(Sitecore.XA.Feature.Redirects.Templates.RedirectMap.ID))).ToArray<Item>();
                        Array.Sort<Item>(array, (IComparer<Item>)new TreeComparer());
                        foreach (Item obj2 in array)
                        {
                            RedirectType result;
                            if (!Enum.TryParse<RedirectType>(obj2[Sitecore.XA.Feature.Redirects.Templates.RedirectMap.Fields.RedirectType], out result))
                            {
                                Log.Info("Redirect map " + obj2.Paths.FullPath + " does not specify redirect type.", (object)this);
                            }
                            else
                            {
                                bool flag1 = MainUtil.GetBool(obj2[Sitecore.XA.Feature.Redirects.Templates.RedirectMap.Fields.PreserveQueryString], false);
                                UrlString urlString = new UrlString()
                                {
                                    Query = obj2[Sitecore.XA.Feature.Redirects.Templates.RedirectMap.Fields.UrlMapping]
                                };
                                foreach (string key in urlString.Parameters.Keys)
                                {
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        string parameter = urlString.Parameters[key];
                                        if (!string.IsNullOrEmpty(parameter))
                                        {
                                            string text = key.ToLowerInvariant();
                                            bool flag2 = text.StartsWith("^", StringComparison.Ordinal) && text.EndsWith("$", StringComparison.Ordinal);
                                            if (!flag2)
                                                text = this.EnsureSlashes(text);
                                            string str = HttpUtility.UrlDecode(parameter.ToLowerInvariant()).TrimStart('^').TrimEnd('$');
                                            mappingsMap.Add(new RedirectMapping()
                                            {
                                                RedirectType = result,
                                                PreserveQueryString = flag1,
                                                Pattern = text,
                                                Target = str,
                                                IsRegex = flag2
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (this.CacheExpiration > 0)
                        HttpRuntime.Cache.Add(this.AllMappingsPrefix, (object)mappingsMap, (CacheDependency)null, DateTime.UtcNow.AddMinutes((double)this.CacheExpiration), TimeSpan.Zero, CacheItemPriority.Normal, (CacheItemRemovedCallback)null);
                }
                return (IList<RedirectMapping>)mappingsMap;
            }
        }

        protected virtual string GetTargetUrl(RedirectMapping mapping, string input)
        {
            string replacement = mapping.Target;
            if (mapping.IsRegex)
                replacement = mapping.Regex.Replace(input, replacement);
            if (mapping.PreserveQueryString)
                replacement += HttpContext.Current.Request.Url.Query;
            if(Uri.TryCreate(replacement, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return replacement;
            }
            
            if (!string.IsNullOrEmpty(this.Context.Site.VirtualFolder))
                replacement = StringUtil.EnsurePostfix('/', this.Context.Site.VirtualFolder) + replacement.TrimStart('/');
            return replacement;
        }

        protected virtual void Redirect301(HttpResponse response, string url)
        {
            HttpCookieCollection cookieCollection = new HttpCookieCollection();
            for (int index = 0; index < response.Cookies.Count; ++index)
            {
                HttpCookie cookie = response.Cookies[index];
                if (cookie != null)
                    cookieCollection.Add(cookie);
            }
            response.Clear();
            for (int index = 0; index < cookieCollection.Count; ++index)
            {
                HttpCookie cookie = cookieCollection[index];
                if (cookie != null)
                    response.Cookies.Add(cookie);
            }
            response.Status = "301 Moved Permanently";
            response.AddHeader("Location", url);
            response.End();
        }

        private string EnsureSlashes(string text) => StringUtil.EnsurePostfix('/', StringUtil.EnsurePrefix('/', text));
    }
}
