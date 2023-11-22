// Decompiled with JetBrains decompiler
// Type: OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext.AddSiteObject
// Assembly: Sitecore.XA.Foundation.Scriban, Version=6.10.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11AE1DEC-A15D-4D6E-9649-838ED84316F4
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.XA.Foundation.Scriban.dll

using Sitecore.XA.Foundation.Multisite;
using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddSiteObject : IGenerateScribanContextProcessor
    {
        protected readonly IMultisiteContext MultisiteContext;
        protected readonly IPageContext PageContext;

        public AddSiteObject(IMultisiteContext multisiteContext, IPageContext pageContext)
        {
            MultisiteContext = multisiteContext;
            PageContext = pageContext;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Add("i_site", MultisiteContext.GetSiteItem(PageContext.Current));
            args.GlobalScriptObject.Add("i_home", MultisiteContext.GetHomeItem(PageContext.Current));
        }
    }
}