namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class EnvironmentExtensions
    {
        public static bool IsContentDeliveryOrStandalone()
        {
            var role = System.Configuration.ConfigurationManager.AppSettings["role:define"];
            return !string.IsNullOrWhiteSpace(role) && (role.Contains("ContentDelivery") || role.Contains("Standalone")) ;
        }

        public static bool IsProductionContentDelivery()
        {
            var environment = System.Configuration.ConfigurationManager.AppSettings["environment:define"];
            return IsContentDeliveryOrStandalone() && environment.Contains("Production");
        }
    }
}