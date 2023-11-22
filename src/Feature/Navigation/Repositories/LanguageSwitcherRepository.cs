using OneWeb.Feature.Navigation.Models;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using Sitecore.DependencyInjection;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.XA.Foundation.Abstractions;

namespace OneWeb.Feature.Navigation.Repositories
{
    public class LanguageSwitcherRepository : VariantsRepository, ILanguageSwitcherRepository
    {
        public LanguageSwitcherModel GetSiteLanguages(Item datasourceItem)
        {
            var languageSwitcherModel = new LanguageSwitcherModel();
            FillBaseProperties(languageSwitcherModel);
            if (datasourceItem == null) return languageSwitcherModel;

            var languageModelList = new List<LanguageModel>();
            var contextItem = ServiceLocator.ServiceProvider.GetService<IContext>().Item;
            var contextSite = ServiceLocator.ServiceProvider.GetService<IContext>().Site;
            var homeItem = SiteExtensions.GetHomeItem();
            languageSwitcherModel.LanguageSwitcherItem = datasourceItem;
            if (datasourceItem.HasChildren)
            {
                var siteList = datasourceItem.GetChildren().ToList();
                foreach (var site in siteList.Where(x => x != null && !string.IsNullOrWhiteSpace(x.GetFieldValue(OneWebInternalSite.Fields.Title_FieldName))))
                {
                    var currentModel = new LanguageModel
                    {
                        Title = site.Fields[OneWebInternalSite.Fields.Title_FieldName].Value
                    };

                    switch (site.TemplateID.ToGuid().ToString())
                    {
                        case OneWebInternalSite.TemplateIdString:
                            var siteGroupingItem = site.GetReferencedItem(OneWebInternalSite.Fields.Link_FieldName);
                            if (siteGroupingItem == null)
                            {
                                break;
                            }

                            var siteName = siteGroupingItem.Fields["SiteName"].Value;
                            var siteInfo = Sitecore.Configuration.Factory.GetSite(siteName);

                            using (new SiteContextSwitcher(siteInfo))
                            {
                                var internalUrlOptions = UrlOptions.DefaultOptions;
                                internalUrlOptions.Language = Language.Parse(siteInfo.Language);
                                internalUrlOptions.LanguageEmbedding = LanguageEmbedding.Always;
                                internalUrlOptions.LanguageLocation = LanguageLocation.QueryString;
                                internalUrlOptions.AlwaysIncludeServerUrl = false;

                                var currentUrl = !contextItem.HasVersionInLanguage(Language.Parse(siteInfo.Language))
                                    ? homeItem.GetItemUrlWithDomain(internalUrlOptions)
                                    : contextItem.GetItemUrlWithDomain(internalUrlOptions);
                                if (siteInfo.Language == Sitecore.Context.Language.Name)
                                {
                                    currentModel.Active = true;
                                }

                                currentModel.Link = new LinkModel()
                                {
                                    Url = currentUrl,
                                    Target = "self",
                                };
                            }
                            break;
                        case OneWebExternalSite.TemplateIdString:
                            currentModel.Link = new LinkModel(site.Fields[OneWebExternalSite.Fields.Link_FieldName]);
                            break;
                        case OneWebLanguageSite.TemplateIdString:
                            var language = site.GetFieldValue(OneWebLanguageSite.Fields.Language_FieldName);
                            if (string.IsNullOrWhiteSpace(language))
                            {
                                break;
                            }
                            Language.TryParse(language, out var parsedLanguage);
                            if (parsedLanguage == null)
                            {
                                break;
                            }

                            var urlOptions = UrlOptions.DefaultOptions;
                            urlOptions.Language = parsedLanguage;
                            urlOptions.LanguageEmbedding = LanguageEmbedding.Always;
                            urlOptions.LanguageLocation = LanguageLocation.QueryString;

                            if (contextItem.HasVersionInLanguage(parsedLanguage))
                            {
                                currentModel.Link = new LinkModel()
                                {
                                    Url = contextItem.GetItemUrl(urlOptions)
                                };
                            }
                            else
                            {
                                currentModel.Link = new LinkModel()
                                {
                                    Url = homeItem.GetItemUrl(urlOptions)
                                };
                            }

                            if (parsedLanguage == Sitecore.Context.Language)
                            {
                                currentModel.Active = true;
                            }
                            break;
                    }

                    languageModelList.Add(currentModel);
                }
                languageSwitcherModel.Languages = languageModelList;
            }
            return languageSwitcherModel;
        }
    }
}