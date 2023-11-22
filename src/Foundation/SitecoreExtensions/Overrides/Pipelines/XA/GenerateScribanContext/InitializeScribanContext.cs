using OneWeb.Foundation.SitecoreExtensions.Extensions.XA;
using Scriban.Runtime;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class InitializeScribanContext : IGenerateScribanContextProcessor
    {
        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject = new ScriptObject();
            args.TemplateContext = new SitecoreTemplateContext(args.RenderingWebEditingParams);
        }
    }
}