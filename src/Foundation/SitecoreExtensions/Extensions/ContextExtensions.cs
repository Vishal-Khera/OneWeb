using Sitecore;
using Sitecore.Data;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class ContextExtensions
    {
        public static string GetContextLanguageName()
        {
            var contextLanguage = Context.Language;
            if (contextLanguage != null) return contextLanguage.Name;
            return Context.Site?.Language != null ? Context.Site.Language : string.Empty;
        }

        public static string GetContextLanguageNameTwoDigitCode()
        {
            var contextLanguage = Context.Language;
            if (contextLanguage != null) return contextLanguage.CultureInfo.TwoLetterISOLanguageName;
            return "en";
        }

        public static string GetContextSiteName()
        {
            var rootPath = Context.Site?.RootPath;
            if (!string.IsNullOrWhiteSpace(rootPath))
            {
                return rootPath.Substring(rootPath.LastIndexOf('/') + 1);
            }

            return Context.Site?.Name;
        }

        public static string GetContextSiteDatabaseName()
        {
            if (Context.Database != null)
                return Context.Database.Name;
            return Context.Site != null ? Context.Site.Database.Name : string.Empty;
        }

        public static Database GetContextSiteDatabase()
        {
            if (Context.Database != null)
                return Context.Database;
            return Context.Site != null ? Context.Site.Database : Sitecore.Configuration.Factory.GetDatabase("web");
        }

        public static string GetContextSiteRootPath()
        {
            return Context.Site != null ? Context.Site.RootPath : string.Empty;
        }

        public static string GetContextSiteStartPath()
        {
            return Context.Site != null ? Context.Site.StartPath : string.Empty;
        }
    }
}