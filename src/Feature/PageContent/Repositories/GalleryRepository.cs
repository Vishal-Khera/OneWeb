using OneWeb.Feature.PageContent.Models;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OneWeb.Feature.PageContent.Repositories
{
    public class GalleryRepository : ModelRepository, IGalleryRepository
    {
        public List<GalleryModel> GetGalleryList()
        {
            var headingList = new List<GalleryModel>();
            var childrens = Sitecore.Context.Item.Axes.GetDescendants();
            var dateItems = childrens.Where(x => x.TemplateID == Sitecore.Data.ID.Parse("{C98363B2-85A5-45B1-B8D5-892A31B0F464}"));
            foreach (var dateItem in dateItems)
            {
                var heading = new GalleryModel();
                heading.Date = dateItem.Fields["Date"].Value;
                var galleryList = new List<GalleryMedia>();
                var galleryItems = dateItem.GetChildren();
                foreach (Item galleryItem in galleryItems)
                {
                    Sitecore.Data.Fields.ImageField imageField = galleryItem.Fields["image"];

                    if (imageField != null && imageField.MediaItem != null)
                    {
                        Sitecore.Data.Items.MediaItem imageg = new Sitecore.Data.Items.MediaItem(imageField.MediaItem);
                        var gallery = new GalleryMedia();
                        gallery.image = MediaManager.GetMediaUrl(imageg);
                        galleryList.Add(gallery);
                    }
                }
                heading.Images = galleryList;
                headingList.Add(heading);               
            }
            headingList = headingList.OrderByDescending(x => x.Date).ToList();
            return headingList;
        }

    }
}