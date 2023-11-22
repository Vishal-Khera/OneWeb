// Decompiled with JetBrains decompiler
// Type: OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext.GenerateScribanContextPipelineArgs
// Assembly: Sitecore.XA.Foundation.Scriban, Version=6.10.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11AE1DEC-A15D-4D6E-9649-838ED84316F4
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.XA.Foundation.Scriban.dll

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Scriban;
using Scriban.Runtime;
using Sitecore.Pipelines;
using Sitecore.XA.Foundation.Mvc.Models;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    [Serializable]
    public class GenerateScribanContextPipelineArgs : PipelineArgs
    {
        private RenderingWebEditingParams _renderingWebEditingParams;

        public GenerateScribanContextPipelineArgs()
        {
        }

        protected GenerateScribanContextPipelineArgs(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ScriptObject GlobalScriptObject { get; set; }

        public TemplateContext TemplateContext { get; set; }

        public bool IsControlEditable { get; set; } = true;

        public bool IsFromComposite { get; set; }

        public RenderingWebEditingParams RenderingWebEditingParams
        {
            get
            {
                var webEditingParams1 = _renderingWebEditingParams;
                if (webEditingParams1 != null)
                    return webEditingParams1;
                var webEditingParams2 = new RenderingWebEditingParams();
                webEditingParams2.DisableWebEditing = !IsControlEditable;
                webEditingParams2.SkipCommonButtons = IsFromComposite;
                var webEditingParams3 = webEditingParams2;
                _renderingWebEditingParams = webEditingParams2;
                return webEditingParams3;
            }
            set => _renderingWebEditingParams = value;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            base.GetObjectData(info, context);
            info.AddValue("TemplateContext", TemplateContext);
        }
    }
}