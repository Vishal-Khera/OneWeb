using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using OneWeb.Feature.Navigation.Models;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.Navigation.Repositories
{
    public class PageNavigationRepository :
        VariantsRepository,
        IPageNavigationRepository
    {

        public PageNavigationRenderingModel GetModel()
        {
            Item item = Sitecore.Context.Item;
            PageNavigationRenderingModel pageNavList = new PageNavigationRenderingModel();
            PageNavigationModel pageNavigationModel = null;
            pageNavList.NavigationTitle = item.GetFieldValue(Templates.Navigable.Fields.NavigationTitle);
            Sitecore.Data.Items.Item[] items= { };
            if (Sitecore.Context.PageMode.IsExperienceEditorEditing==false || Sitecore.Context.PageMode.IsExperienceEditor==false)
            {
                var pageDesignValue = item.Fields["Page Design"].Value;
                Item pageDesign = Sitecore.Context.Database.GetItem(pageDesignValue);
                Sitecore.Data.Items.Item myItem = (Sitecore.Data.Items.Item)pageDesign;
                Sitecore.Data.Fields.MultilistField multiselectField = pageDesign.Fields["PartialDesigns"];
                items = multiselectField.GetItems();
            }

            if (item != null)
            {
                if ((item.Visualization.GetRenderings(Sitecore.Context.Device, true).Where(r => r.RenderingID == Sitecore.Data.ID.Parse(Templates.PageNavigable.RenderingId))).Any())
                {
                    var renderings = item.Visualization.GetRenderings(Sitecore.Context.Device, true).Where(r => r.RenderingID == Sitecore.Data.ID.Parse(Templates.PageNavigable.RenderingId));
                    if (renderings != null)
                    {
                        foreach (var rendering in renderings)
                        {
                            var parm = HttpUtility.ParseQueryString(rendering.Settings.Parameters);
                            pageNavigationModel = new PageNavigationModel
                            {
                                PageNavTitle = parm[OneWebSectionRenderingParameters.Fields.Title_FieldName],
                                PageNavId = parm[OneWebSectionRenderingParameters.Fields.SectionId_FieldName]
                            };
                            if (pageNavigationModel != null)
                                pageNavList.PageNavigationList.Add(pageNavigationModel);
                        }
                    }
                }
                else
                {
                    foreach (var itemMultiList in items)
                    {
                        if ((itemMultiList.Visualization.GetRenderings(Sitecore.Context.Device, true).Where(r => r.RenderingID == Sitecore.Data.ID.Parse(Templates.PageNavigable.RenderingId))).Any())
                        {
                            var renderings = itemMultiList.Visualization.GetRenderings(Sitecore.Context.Device, true).Where(r => r.RenderingID == Sitecore.Data.ID.Parse(Templates.PageNavigable.RenderingId));
                            if (renderings != null)
                            {
                                foreach (var rendering in renderings)
                                {
                                    var parm = HttpUtility.ParseQueryString(rendering.Settings.Parameters);
                                    pageNavigationModel = new PageNavigationModel
                                    {
                                        PageNavTitle = parm[OneWebSectionRenderingParameters.Fields.Title_FieldName],
                                        PageNavId = parm[OneWebSectionRenderingParameters.Fields.SectionId_FieldName]
                                    };
                                    if (pageNavigationModel != null)
                                        pageNavList.PageNavigationList.Add(pageNavigationModel);
                                }
                            }
                        }
                    }
                }
            }
            if (pageNavList != null)
                FillBaseProperties(pageNavList);
            return pageNavList;
        }
    }
}
