using Sitecore.XA.Foundation.DynamicPlaceholders.Models;

namespace OneWeb.Feature.PageStructure.Models
{
    public class SlideWrapperRenderingModel : PageStructureRenderingModel
    {
        public float SlidesToShowDesktop { get; set; }
        public float SlidesToShowTab { get; set; }
        public float SlidesToShowMobile { get; set; }
        public bool ShowArrows { get; set; }
        public bool ShowDots { get; set; }
        public bool Autoplay { get; set; }
        public int AutoplaySpeed { get; set; }
        public string SliderClass { get; set; }
        public bool CarouselDesktopDisable { get; set; }
        public bool InfiniteLoop { get; set; }
        public float RowsMobile { get; set; }
        public float RowsTablet { get; set; }
        public float RowsDesktop { get; set; }

    }
}