// Decompiled with JetBrains decompiler
// Type: OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext.AddQueryFunction
// Assembly: Sitecore.XA.Foundation.Scriban, Version=6.10.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11AE1DEC-A15D-4D6E-9649-838ED84316F4
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.XA.Foundation.Scriban.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Scriban.Runtime;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.TokenResolution;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddQueryFunction : IGenerateScribanContextProcessor
    {
        protected readonly ITokenResolver TokenResolver;

        public AddQueryFunction(ITokenResolver tokenResolver)
        {
            TokenResolver = tokenResolver;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Import("sc_query", (AddQueryDelegate)((item, query) =>
            {
                query = TokenResolver.Resolve(query, item);
                return (item.Axes.SelectItems(query) ?? Array.Empty<Item>()).Where(
                    i => i != null);
            }));
        }

        private delegate IEnumerable<Item> AddQueryDelegate(Item inputItem, string inputString);
    }
}