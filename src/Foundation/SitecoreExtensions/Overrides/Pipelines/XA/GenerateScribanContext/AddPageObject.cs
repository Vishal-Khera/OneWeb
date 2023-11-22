// Decompiled with JetBrains decompiler
// Type: OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext.AddPageObject
// Assembly: Sitecore.XA.Foundation.Scriban, Version=6.10.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11AE1DEC-A15D-4D6E-9649-838ED84316F4
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.XA.Foundation.Scriban.dll

using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddPageObject : IGenerateScribanContextProcessor
    {
        protected readonly IPageContext PageContext;

        public AddPageObject(IPageContext pageContext)
        {
            PageContext = pageContext;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Add("i_page", PageContext.Current);
        }
    }
}