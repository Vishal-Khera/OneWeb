using OneWeb.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneWeb.Foundation.Search.Models;
using OneWeb.Foundation.Serialization;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace OneWeb.Foundation.Search.Controllers
{
    public class MappingController : Controller
    {
        public JsonResult GetMapping(string mappingId, string primaryFacet, string secondaryFacet)
        {
            if (!string.IsNullOrWhiteSpace(mappingId))
            {
                var mappingItem = Sitecore.Context.Database.GetItem(ShortID.Parse(mappingId).ToID(),
                    Language.Parse(ContextExtensions.GetContextLanguageName()));
                if (mappingItem != null)
                {
                    var mappingObject = new List<FilterMapping>();
                    foreach (Item mappingChild in mappingItem.GetChildren())
                    {
                        var mapping = new FilterMapping()
                        {
                            PrimaryFilter = new Models.Filter()
                            {
                                Id = mappingChild.ID.ToShortID().ToString(),
                                Title = mappingChild.GetFieldValue(BaseContent.Fields.Title_FieldName),
                            },
                            SecondaryFilter = mappingChild.Children.Select(x => new Models.Filter()
                            {
                                Id = x.ID.ToShortID().ToString(),
                                Title = x.GetFieldValue(BaseContent.Fields.Title_FieldName),
                            }).ToList()
                        };
                        mappingObject.Add(mapping);
                    }

                    return Json(new { Data = mappingObject}, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Data = string.Empty }, JsonRequestBehavior.AllowGet);
        }
    }
}