using Sitecore.XA.Foundation.Abstractions;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddContext : IGenerateScribanContextProcessor
    {
        protected readonly IContext Context;
        protected readonly IPageMode PageMode;

        public AddContext(IContext context, IPageMode iPageMode)
        {
            Context = context;
            PageMode = iPageMode;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Add("o_context", Context);
            args.GlobalScriptObject.Add("o_pagemode", PageMode);
        }
    }
}