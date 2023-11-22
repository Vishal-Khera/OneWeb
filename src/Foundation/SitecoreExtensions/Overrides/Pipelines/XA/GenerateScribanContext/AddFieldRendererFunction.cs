// Decompiled with JetBrains decompiler
// Type: OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext.AddFieldRendererFunction
// Assembly: Sitecore.XA.Foundation.Scriban, Version=6.10.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11AE1DEC-A15D-4D6E-9649-838ED84316F4
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.XA.Foundation.Scriban.dll

using System.Collections.Specialized;
using Scriban.Runtime;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Abstractions;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddFieldRendererFunction : FieldRendererBase, IGenerateScribanContextProcessor
    {
        protected readonly IPageMode PageMode;

        public AddFieldRendererFunction(IPageMode iPageMode)
        {
            PageMode = iPageMode;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            RenderingWebEditingParams = args.RenderingWebEditingParams;
            RenderField function = RenderFieldImpl;
            args.GlobalScriptObject.Import("sc_field", function);
        }

        public string RenderFieldImpl(Item item, object field, ScriptArray parameters = null)
        {
            var parameters1 = new NameValueCollection();
            if (parameters != null)
                foreach (var parameter in parameters)
                    if (parameter is ScriptArray scriptArray && scriptArray.Count > 1)
                        parameters1.Add(scriptArray[0].ToString(), scriptArray[1].ToString());
            string fieldName = null;
            switch (field)
            {
                case string _:
                    fieldName = field as string;
                    break;
                case ScriptArray _:
                    using (var enumerator = (field as ScriptArray).GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                            if (enumerator.Current is string current)
                            {
                                fieldName = fieldName ?? current;
                                var experienceEditorEditing = PageMode.IsExperienceEditorEditing;
                                var field1 = item.Fields[current];
                                if (field1 != null)
                                {
                                    if (experienceEditorEditing)
                                    {
                                        fieldName = current;
                                        break;
                                    }

                                    if (!string.IsNullOrWhiteSpace(field1.GetValue(true)))
                                    {
                                        fieldName = current;
                                        break;
                                    }
                                }
                            }

                        break;
                    }
            }

            return CreateFieldRenderer(item, fieldName, parameters1).Render();
        }

        private delegate string RenderField(Item item, object field, ScriptArray parameters = null);
    }
}