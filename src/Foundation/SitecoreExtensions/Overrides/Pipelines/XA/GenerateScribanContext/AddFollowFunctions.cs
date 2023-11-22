// Decompiled with JetBrains decompiler
// Type: OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext.AddFollowFunctions
// Assembly: Sitecore.XA.Foundation.Scriban, Version=6.10.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11AE1DEC-A15D-4D6E-9649-838ED84316F4
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.XA.Foundation.Scriban.dll

using System.Collections.Generic;
using System.Linq;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Scriban.Runtime;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddFollowFunctions : IGenerateScribanContextProcessor
    {
        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Import("sc_follow",
                (AddFollowFunctionsDelegate)((item, fieldName) =>
                    ItemExtensions.GetTargetItems(item, fieldName).FirstOrDefault()));
            args.GlobalScriptObject.Import("sc_followmany",
                (AddFollowFunctionsManyDelegate)ItemExtensions.GetTargetItems);
        }

        private delegate Item AddFollowFunctionsDelegate(Item inputItem, string inputString);

        private delegate IEnumerable<Item> AddFollowFunctionsManyDelegate(Item inputItem, string inputString);
    }
}