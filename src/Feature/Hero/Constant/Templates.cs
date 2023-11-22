using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneWeb.Feature.HeroBanner.Constant
{
    public struct Templates
    {
        public struct OneWebBannerCarousel
        {
            public static ID TemplateId = ID.Parse("{2DCD3B85-EC83-4DE1-87FA-44E00F1E588F}");
        }
        public struct OneWebImageListBanner
        {
            public static ID TemplateId = ID.Parse("{3F2DC1DC-7587-4510-A5F2-F97D7B0AD768}");
        }
        public struct OneWebVideoBanner
        {
            public static ID TemplateId = ID.Parse("{81B6BC15-A405-44BD-8CAF-236E785BA28C}");
        }
    }
}