using Microsoft.Extensions.DependencyInjection;
using Scriban.Runtime;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.MarkupDecorator.Extensions;
using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddDecorationFunction : IGenerateScribanContextProcessor
    {
        protected readonly IPageContext PageContext;

        public AddDecorationFunction(IPageContext pageContext)
        {
            PageContext = pageContext;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Import("sc_decorate",
                (AddDecorationDelegate)(cssClasName =>
                    new RenderingMarkupBuilder(PageContext, ServiceLocator.ServiceProvider.GetService<IRendering>(),
                        cssClasName).ToHtmlString()));
        }

        private delegate string AddDecorationDelegate(string variantName);
    }
}