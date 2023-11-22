using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Data.Items;
using System.Linq;
using OneWeb.Foundation.SitecoreExtensions.Models.Integration;
using BaseIntegration = OneWeb.Foundation.Serialization.BaseIntegration;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public class IntegrationsExtensions
    {
        public static GoogleAnalyticsModel GetGoogleAnalyticsModel()
        {
            Item integration = SiteExtensions.GetSettingsItem();
            var googleAnlyticsPartner = integration.GetMultiListItems(SiteConfigurations.Fields.Integrations_FieldName).
                FirstOrDefault(x => x.TemplateID == GoogleAnalyticsIntegration.TemplateId);
            if (googleAnlyticsPartner != null)
            {
                return new GoogleAnalyticsModel
                {
                    Id = googleAnlyticsPartner.GetFieldValue(GoogleAnalyticsIntegration.Fields.GoogleAnalyticsId_FieldName),
                    Name = googleAnlyticsPartner.GetFieldValue(BaseIntegration.Fields.Name_FieldName),
                    OneTrustCategoryId = googleAnlyticsPartner.GetFieldValue(BaseIntegration.Fields.OneTrustCategoryId_FieldName)
                };
            }
            return new GoogleAnalyticsModel();
        }

        public static GoogleTagManagerModel GetTagManagerModel()
        {
            Item integration = SiteExtensions.GetSettingsItem();
            var googleTagManagerPartner = integration
                .GetMultiListItems(SiteConfigurations.Fields.Integrations_FieldName)
                .FirstOrDefault(x => x.TemplateID == GoogleTagManagerIntegration.TemplateId);
            if (googleTagManagerPartner != null)
            {
                return new GoogleTagManagerModel
                {
                    Id = googleTagManagerPartner.GetFieldValue(GoogleTagManagerIntegration.Fields.GoogleTagManagerId_FieldName),
                    Name = googleTagManagerPartner.GetFieldValue(BaseIntegration.Fields.Name_FieldName),
                    OneTrustCategoryId = googleTagManagerPartner.GetFieldValue(BaseIntegration.Fields.OneTrustCategoryId_FieldName)
                };
            }
            return new GoogleTagManagerModel();
        }
        public static OneTrustModel GetOneTrustModel()
        {
            Item integration = SiteExtensions.GetSettingsItem();
            var oneTrustPartner = integration.GetMultiListItems(SiteConfigurations.Fields.Integrations_FieldName)
                .FirstOrDefault(x => x.TemplateID == OneTrustIntegration.TemplateId);
             if (oneTrustPartner != null)
            {
                return new OneTrustModel
                {
                    Id = EnvironmentExtensions.IsProductionContentDelivery()
                        ? oneTrustPartner.GetFieldValue(OneTrustIntegration.Fields.OneTrustID_FieldName)
                        : oneTrustPartner.GetFieldValue(OneTrustIntegration.Fields.OneTrustID_FieldName) + "-test",
                    DisableCookieAutoBlocking = oneTrustPartner.GetCheckboxStatus(OneTrustIntegration.Fields.DisableCookieAutoBlocking_FieldName),
                    ReloadOnConsentChange= oneTrustPartner.GetCheckboxStatus(OneTrustIntegration.Fields.ReloadOnConsentChange_FieldName)
                };
            }
            return new OneTrustModel();
        }
    }
}