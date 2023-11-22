// Decompiled with JetBrains decompiler
// Type: OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext.AddRenderingData
// Assembly: Sitecore.XA.Foundation.Scriban, Version=6.10.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11AE1DEC-A15D-4D6E-9649-838ED84316F4
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.XA.Foundation.Scriban.dll

using Microsoft.Extensions.DependencyInjection;
using Scriban.Runtime;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddRenderingData : IGenerateScribanContextProcessor
    {
        public void Process(GenerateScribanContextPipelineArgs args)
        {
            var rendering = ServiceLocator.ServiceProvider.GetService<IRendering>();
            if (rendering == null)
                return;
            args.GlobalScriptObject.Add("i_datasource", rendering.DataSourceItem);
            args.GlobalScriptObject.Import("sc_parameter",
                (AddRenderingDataDelegate)(parameterName =>
                    !rendering.Parameters.Contains(parameterName)
                        ? rendering.Properties[parameterName]
                        : rendering.Parameters[parameterName]));
        }

        private delegate string AddRenderingDataDelegate(string inputString);
    }
}