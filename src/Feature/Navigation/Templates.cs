using Sitecore.Data;

namespace OneWeb.Feature.Navigation
{
    public struct Templates
    {
        public struct Redirect
        {
            public struct Fields
            {
                public static ID RedirectUrl = ID.Parse("{22753447-BE25-4035-A3C9-7F875AFE8E57}");
            }
        }

        public struct Navigable
        {
            public static ID TemplateId = ID.Parse("{371D5FBB-5498-4D94-AB2B-E3B70EEBE78C}");

            public struct Fields
            {
                public static string NavigationTitle = "NavigationTitle";
                public static string NavigationFilter = "NavigationFilter";
                public static string NavigationClass = "NavigationClass";
            }
        }

        public struct PageNavigable
        {
            public static ID RenderingId = ID.Parse("{7E1BC643-D4C9-41AF-AF11-F32388B65BE2}");
        }
    }
}