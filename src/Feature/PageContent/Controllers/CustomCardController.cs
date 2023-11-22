using OneWeb.Feature.PageContent.Models;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class CustomCardController : Controller
    {
        // GET: Card
        public ActionResult CustomCard()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item ?? Sitecore.Context.Item;
            ImageField imageField = datasourceItem.Fields["Image"];
            Sitecore.Data.Items.MediaItem image = new Sitecore.Data.Items.MediaItem(imageField.MediaItem);
            // Create a new instance of the CardModel
            var model = new CustomCardModel
            {
                Title = datasourceItem.Fields["Title"].Value,
                Description = datasourceItem.Fields["Description"].Value,
                ImageUrl = MediaManager.GetMediaUrl(image)
            };
            return View(model);

        }
    }
}