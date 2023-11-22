using OneWeb.Feature.PageStructure.Models;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.DynamicPlaceholders.Repositories;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Globalization;

namespace OneWeb.Feature.PageStructure.Repositories
{
    public class PageStructureRepository :
     DynamicPlaceholdersRepository,
     IPageStructureRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var pageStructureRenderingModel = new PageStructureRenderingModel();
            base.FillBaseProperties(pageStructureRenderingModel);
            pageStructureRenderingModel.BackgroundColor = Rendering.Parameters[Foundation.Serialization.Styling.Fields.BackgroundColor_FieldName];
            return pageStructureRenderingModel;
        }

        public SlideWrapperRenderingModel GetSlideWrapper()
        {
            var slideWrapperRenderingModel = new SlideWrapperRenderingModel();
            base.FillBaseProperties(slideWrapperRenderingModel);
            try
            {
                slideWrapperRenderingModel.BackgroundColor = Rendering.Parameters[Styling.Fields.BackgroundColor_FieldName];

                slideWrapperRenderingModel.InfiniteLoop = Rendering.Parameters[BaseSliderRenderingParameters.Fields.InfiniteLoop_FieldName] == "1";

                slideWrapperRenderingModel.Autoplay = Rendering.Parameters[BaseSliderRenderingParameters.Fields.Autoplay_FieldName] == "1";

                if (int.TryParse(Rendering.Parameters[BaseSliderRenderingParameters.Fields.AutoplaySpeed_FieldName], out var autoplaySpeed))
                {
                    slideWrapperRenderingModel.AutoplaySpeed = autoplaySpeed;
                }

                slideWrapperRenderingModel.ShowArrows = Rendering.Parameters[BaseSliderRenderingParameters.Fields.ShowArrows_FieldName] == "1";
                slideWrapperRenderingModel.ShowDots = Rendering.Parameters[BaseSliderRenderingParameters.Fields.ShowDots_FieldName] == "1";


                slideWrapperRenderingModel.SlidesToShowDesktop =
                    float.TryParse(Rendering.Parameters[BaseSliderRenderingParameters.Fields.SlidesToShow_FieldName],
                    NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var slidesToShow) && slidesToShow > 0
                        ? slidesToShow
                        : 1;

                slideWrapperRenderingModel.SlidesToShowTab =
                    float.TryParse(Rendering.Parameters[BaseSliderRenderingParameters.Fields.SlidesToShowTab_FieldName],
                     NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var slidesToShowTab) && slidesToShowTab > 0
                        ? slidesToShowTab
                        : 1;

                slideWrapperRenderingModel.SlidesToShowMobile =
                    float.TryParse(
                        Rendering.Parameters[BaseSliderRenderingParameters.Fields.SlidesToShowMobile_FieldName],
                         NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var slidesToShowMobile) && slidesToShowMobile > 0
                        ? slidesToShowMobile
                        : 1;
                var customSliderClassItemId = Rendering.Parameters[BaseSliderRenderingParameters.Fields.CustomSliderClass_FieldName];
                if (!string.IsNullOrEmpty(customSliderClassItemId))
                {
                    Item customSliderClassItem = ItemExtensions.GetItem(customSliderClassItemId);
                    slideWrapperRenderingModel.SliderClass = string.IsNullOrEmpty(customSliderClassItem["Value"]) ? slideWrapperRenderingModel.Rendering.RenderingCssClass : customSliderClassItem["Value"];
                }
                else
                {
                    slideWrapperRenderingModel.SliderClass = slideWrapperRenderingModel.Rendering.RenderingCssClass;
                }

                slideWrapperRenderingModel.CarouselDesktopDisable = Rendering.Parameters[BaseSliderRenderingParameters.Fields.CarouselDesktopDisable_FieldName] == "1";
                if (slideWrapperRenderingModel.CarouselDesktopDisable == true)
                {
                    slideWrapperRenderingModel.SliderClass = slideWrapperRenderingModel.SliderClass + " ow-disable-carousal-desktop";
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError("Error parsing rendering parameters for Slide Wrapper", ex);
            }
            return slideWrapperRenderingModel;
        }

        public SlideWrapperRenderingModel GetSlideWidget()
        {
            var slideWrapperRenderingModel = new SlideWrapperRenderingModel();
            base.FillBaseProperties(slideWrapperRenderingModel);
            try
            {
                slideWrapperRenderingModel.BackgroundColor = Rendering.Parameters[Styling.Fields.BackgroundColor_FieldName];

                slideWrapperRenderingModel.Autoplay = Rendering.Parameters[BaseSliderRenderingParameters.Fields.Autoplay_FieldName] == "1";

                slideWrapperRenderingModel.InfiniteLoop = Rendering.Parameters[BaseSliderRenderingParameters.Fields.InfiniteLoop_FieldName] == "1";

                if (int.TryParse(Rendering.Parameters[BaseSliderRenderingParameters.Fields.AutoplaySpeed_FieldName], out var autoplaySpeed))
                {
                    slideWrapperRenderingModel.AutoplaySpeed = autoplaySpeed;
                }

                slideWrapperRenderingModel.ShowArrows = Rendering.Parameters[BaseSliderRenderingParameters.Fields.ShowArrows_FieldName] == "1";
                slideWrapperRenderingModel.ShowDots = Rendering.Parameters[BaseSliderRenderingParameters.Fields.ShowDots_FieldName] == "1";


                slideWrapperRenderingModel.SlidesToShowDesktop =
                    float.TryParse(Rendering.Parameters[BaseSliderRenderingParameters.Fields.SlidesToShow_FieldName],
                    NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var slidesToShow) && slidesToShow > 0
                        ? slidesToShow
                        : 1;

                slideWrapperRenderingModel.SlidesToShowTab =
                    float.TryParse(Rendering.Parameters[BaseSliderRenderingParameters.Fields.SlidesToShowTab_FieldName],
                      NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var slidesToShowTab) && slidesToShowTab > 0
                        ? slidesToShowTab
                        : 1;

                slideWrapperRenderingModel.SlidesToShowMobile =
                    float.TryParse(
                        Rendering.Parameters[BaseSliderRenderingParameters.Fields.SlidesToShowMobile_FieldName],
                          NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var slidesToShowMobile) && slidesToShowMobile > 0
                        ? slidesToShowMobile
                        : 1;

                slideWrapperRenderingModel.RowsMobile =
                     float.TryParse(
                         Rendering.Parameters[OneWebSlideWidgetRenderingParameters.Fields.RowsMobile_FieldName],
                             NumberStyles.Any, CultureInfo.InvariantCulture,
                         out var rowswMobile) && rowswMobile > 0
                         ? rowswMobile
                         : 1;
                slideWrapperRenderingModel.RowsTablet =
                    float.TryParse(
                        Rendering.Parameters[OneWebSlideWidgetRenderingParameters.Fields.RowsTablet_FieldName],
                          NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var rowsTablet) && rowsTablet > 0
                        ? rowsTablet
                        : 1;
                slideWrapperRenderingModel.RowsDesktop =
                    float.TryParse(
                        Rendering.Parameters[OneWebSlideWidgetRenderingParameters.Fields.RowsDesktop_FieldName],
                          NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var rowsDesktop) && rowsDesktop > 0
                        ? rowsDesktop
                        : 1;

                var customSliderClassItemId = Rendering.Parameters[BaseSliderRenderingParameters.Fields.CustomSliderClass_FieldName];
                if (!string.IsNullOrEmpty(customSliderClassItemId))
                {
                    Item customSliderClassItem = ItemExtensions.GetItem(customSliderClassItemId);
                    slideWrapperRenderingModel.SliderClass = string.IsNullOrEmpty(customSliderClassItem["Value"]) ? slideWrapperRenderingModel.Rendering.RenderingCssClass : customSliderClassItem["Value"];
                }
                else
                {
                    slideWrapperRenderingModel.SliderClass = slideWrapperRenderingModel.Rendering.RenderingCssClass;
                }

                slideWrapperRenderingModel.CarouselDesktopDisable = Rendering.Parameters[BaseSliderRenderingParameters.Fields.CarouselDesktopDisable_FieldName] == "1";
                if (slideWrapperRenderingModel.CarouselDesktopDisable == true)
                {
                    slideWrapperRenderingModel.SliderClass = slideWrapperRenderingModel.SliderClass + " ow-disable-carousal-desktop";
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError("Error parsing rendering parameters for Slide Wrapper", ex);
            }
            return slideWrapperRenderingModel;
        }
    }
}
