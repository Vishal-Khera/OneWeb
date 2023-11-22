using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.XA.Feature.SiteMetadata.Enums;
using Sitecore.XA.Feature.SiteMetadata.Sitemap;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using System.Linq;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.DI.Sitemap
{
    public class CustomSitemapGenerator : SitemapGenerator
    {
        protected override StringBuilder BuildMultilanguageSitemap(IEnumerable<Item> childrenTree, SitemapLinkOptions options)
        {
            UrlOptions urlOptions1 = base.GetUrlOptions();
            SitemapLinkOptions options1 = new SitemapLinkOptions(options.Scheme, urlOptions1, options.TargetHostname);
            UrlOptions urlOptions2 = (UrlOptions)urlOptions1.Clone();
            urlOptions2.LanguageEmbedding = LanguageEmbedding.Always;
            SitemapLinkOptions options2 = new SitemapLinkOptions(options.Scheme, urlOptions2, options.TargetHostname);
            List<XElement> xelementList1 = new List<XElement>();
            var siteSetting = SiteExtensions.GetSettingsItem();
            if (childrenTree != null)
            {
                var disctictItems = childrenTree.GroupBy(item => item.ID).Select(group => group.First());
                if (disctictItems != null && disctictItems.Count() > 0)
                {
                    foreach (Item obj1 in disctictItems)
                    {
                        SitemapChangeFrequency sitemapChangeFrequency = obj1.Fields[Sitecore.XA.Feature.SiteMetadata.Templates.Sitemap._Sitemap.Fields.ChangeFrequency].ToEnum<SitemapChangeFrequency>();
                        if (sitemapChangeFrequency == SitemapChangeFrequency.DoNotInclude)
                        {
                            continue;
                        }

                        List<XElement> xelementList2 = new List<XElement>();
                        foreach (Language language in obj1.Languages)
                        {
                            Item obj2 = obj1.Database.GetItem(obj1.ID, language);
                            if (obj2.Versions.Count > 0)
                            {
                                options2.UrlOptions.Language = language;
                                XElement xelement = base.BuildAlternateLinkElement(base.GetFullLink(obj2, options2), language.CultureInfo.Name);
                                xelementList2.Add(xelement);
                            }
                        }
                        options1.UrlOptions.Language = obj1.Language;
                        XElement xelement1 = base.BuildPageElement(base.GetFullLink(obj1, options1), base.GetUpdatedDate(obj1), sitemapChangeFrequency.ToString().ToLowerInvariant(), base.GetPriority(obj1), (IEnumerable<XElement>)xelementList2);
                        xelementList1.Add(xelement1);
                    }
                }
            }
            XDocument xdocument = base.BuildXmlDocument((IEnumerable<XElement>)xelementList1);
            StringBuilder stringBuilder = new StringBuilder();
            using (TextWriter textWriter = (TextWriter)new StringWriter(stringBuilder))
                xdocument.Save(textWriter);
            base.FixDeclaration(stringBuilder);
            return stringBuilder;
        }
    }
}