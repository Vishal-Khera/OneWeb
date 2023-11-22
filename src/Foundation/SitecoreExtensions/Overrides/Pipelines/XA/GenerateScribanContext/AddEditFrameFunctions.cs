// Decompiled with JetBrains decompiler
// Type: OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext.AddEditFrameFunctions
// Assembly: Sitecore.XA.Foundation.Scriban, Version=6.10.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11AE1DEC-A15D-4D6E-9649-838ED84316F4
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.XA.Foundation.Scriban.dll

using System.IO;
using System.Web.UI;
using Scriban.Runtime;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext
{
    public class AddEditFrameFunctions : IGenerateScribanContextProcessor
    {
        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Import("sc_editframe", (AddEditFrameDelegate)((datasource, buttons) =>
            {
                if (!Context.PageMode.IsExperienceEditorEditing)
                    return string.Empty;
                using (var editFrame = new EditFrame
                       {
                           DataSource = datasource.ID.ToString(),
                           Buttons = buttons
                       })
                {
                    using (var writer = new StringWriter())
                    {
                        using (var output = new HtmlTextWriter(writer))
                        {
                            editFrame.RenderFirstPart(output);
                        }

                        return writer.ToString();
                    }
                }
            }));
            args.GlobalScriptObject.Import("sc_endeditframe",
                (AddEditFrameDelegate2)(() => !Context.PageMode.IsExperienceEditorEditing ? string.Empty : "</div>"));
        }

        private delegate string AddEditFrameDelegate(Item datasource, string buttons);

        private delegate string AddEditFrameDelegate2();
    }
}