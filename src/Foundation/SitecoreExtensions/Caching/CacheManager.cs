using System;
using System.Runtime.Caching;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Events;

namespace OneWeb.Foundation.SitecoreExtensions.Caching
{
    public static class CacheManager
    {
        private static readonly Cache OneWebCache = new Cache("oneweb-cache", StringUtil.ParseSizeString("50MB"));

        static CacheManager()
        {
            Event.Subscribe("publish:end", ClearCache);
            Event.Subscribe("publish:end:remote", ClearCache);
        }

        private static void ClearCache(object sender, EventArgs eventArgs)
        {
            OneWebCache.Clear();
        }

        public static T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, int cacheSeconds = 300) where T : class
        {
            if (OneWebCache == null || !EnvironmentExtensions.IsContentDeliveryOrStandalone()) return getItemCallback();
            var site = ContextExtensions.GetContextSiteName();
            var database = ContextExtensions.GetContextSiteDatabaseName();
            var language = ContextExtensions.GetContextLanguageName();
            cacheKey = $"{site}_{database}_{language}_{cacheKey}";

            T returnValue = OneWebCache.GetValue(cacheKey) as T;
            if (returnValue == null)
            {
                returnValue = getItemCallback();
                OneWebCache.Add(cacheKey, returnValue, DateTime.UtcNow.AddSeconds(cacheSeconds));
            }
            return returnValue;
        }

    }
}