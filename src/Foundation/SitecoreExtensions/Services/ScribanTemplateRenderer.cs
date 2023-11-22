using OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext;
using Scriban;
using Sitecore.Abstractions;
using Sitecore.XA.Foundation.Mvc.Models;

namespace OneWeb.Foundation.SitecoreExtensions.Services
{
    public class ScribanTemplateRenderer : IScribanTemplateRenderer
    {
        public ScribanTemplateRenderer(BaseCorePipelineManager pipelineManager)
        {
            PipelineManager = pipelineManager;
        }

        protected BaseCorePipelineManager PipelineManager { get; }

        public virtual TemplateContext GetTemplateContext(
            bool isControlEditable,
            RenderingWebEditingParams webEditingParameters)
        {
            var args = new GenerateScribanContextPipelineArgs
            {
                IsControlEditable = isControlEditable,
                RenderingWebEditingParams = webEditingParameters
            };
            PipelineManager.Run("generateScribanContext", args);
            if (args.GlobalScriptObject != null)
                args.TemplateContext.PushGlobal(args.GlobalScriptObject);
            return args.TemplateContext;
        }

        public virtual Template Parse(string template, string name)
        {
            return Template.Parse(template, name);
        }

        public virtual string Render(Template template, TemplateContext context)
        {
            return template.Render(context);
        }
    }
}