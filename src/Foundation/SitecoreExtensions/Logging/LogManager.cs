using System;
using System.Runtime.CompilerServices;
using log4net;

namespace OneWeb.Foundation.SitecoreExtensions.Logging
{
    public static class LogManager
    {
        private static readonly ILog Log =
            Sitecore.Diagnostics.LoggerFactory.GetLogger("Sitecore.Diagnostics.Integration");

        public static void LogInfo(string message ,[CallerMemberName] string caller = "")
        {
            Log.Info($"OneWeb Logger >> Info{Environment.NewLine}>> {caller} >> {message}");
        }

        public static void LogWarning(string message, [CallerMemberName] string caller = "")
        {
            Log.Warn($"OneWeb Logger >> Warning{Environment.NewLine}>> {caller} >> {message}");
        }

        public static void LogError(string message, Exception ex,[CallerMemberName] string caller = "")
        {
            Log.Error($"OneWeb Logger >> Error{Environment.NewLine}>> {caller}{Environment.NewLine}>> {message}{Environment.NewLine}>> {ex.Message}{Environment.NewLine}>> {ex.StackTrace}");
        }

        public static void LogError(string message, [CallerMemberName] string caller = "")
        {
            Log.Error($"OneWeb Logger >> Error{Environment.NewLine}>> {caller}{Environment.NewLine}>> {message}");
        }

        public static void LogDebug(string message, [CallerMemberName] string caller = "")
        {
            Log.Debug($"OneWeb Logger >> Debug{Environment.NewLine}>> {caller} >> {message}");
        }
    }
}