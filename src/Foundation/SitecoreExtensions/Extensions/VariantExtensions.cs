using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Renderers;
using Sitecore.XA.Foundation.Variants.Abstractions.Services;
using System.Text;
using System.Web;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class VariantExtensions
    {
        private static readonly IVariantRenderer VariantRenderer = ServiceLocator.ServiceProvider.GetService<IVariantRenderer>();
        private static IVariantFieldParser FieldParser => ServiceLocator.ServiceProvider.GetService<IVariantFieldParser>();

        public static HtmlString RenderVariant(Item item, Item variantItem, object model = null)
        {
            var htmlString = new StringBuilder();
            var variantFields = FieldParser.ParseVariantFields(variantItem);

            VariantRenderer.SetParams(new RendererParameters()
            {
                RendererMode = RendererMode.Html,
                Model = model,
            });

            foreach (var variantField in variantFields)
            {
                htmlString.Append(VariantRenderer.RenderVariantField(variantField, item));
            }

            return new HtmlString(htmlString.ToString());
        }
    }
}