// Decompiled with JetBrains decompiler
// Type: Sitecore.Shell.Applications.ContentEditor.NameValue
// Assembly: Sitecore.Kernel, Version=13.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA8A253-0368-41B5-844C-99E9068D32BA
// Assembly location: C:\Projects\OneWeb\packages\Sitecore.Kernel.9.2.0\lib\net471\Sitecore.Kernel.dll
// XML documentation location: C:\Projects\OneWeb\packages\Sitecore.Kernel.9.2.0\lib\net471\Sitecore.Kernel.xml

using System;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.HtmlControls.Data;
using Sitecore.Web.UI.Sheer;

namespace OneWeb.Foundation.SitecoreExtensions.Classes.Fields
{
    /// <summary>Represents a Text field.</summary>
    public class OneWebNameValueList : Input
    {

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="output">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.
        /// </param>
        protected override void DoRender(HtmlTextWriter output)
        {
            Assert.ArgumentNotNull((object)output, nameof(output));
            this.SetWidthAndHeightStyle();
            output.Write("<div" + this.ControlAttributes + ">");
            this.RenderChildren(output);
            output.Write("</div>");
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"></see> object that contains the event data.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull((object)e, nameof(e));
            base.OnLoad(e);
            if (Sitecore.Context.ClientPage.IsEvent)
                this.LoadValue();
            else
                this.BuildControl();
        }

        /// <summary>Parameters the change.</summary>
        protected void ParameterChange()
        {
            ClientPage clientPage = Sitecore.Context.ClientPage;
            if (clientPage.ClientRequest.Source == StringUtil.GetString(clientPage.ServerProperties[this.ID + "_LastParameterID"]) && !string.IsNullOrEmpty(clientPage.ClientRequest.Form[clientPage.ClientRequest.Source]))
            {
                string str = this.BuildParameterKeyValue(string.Empty, string.Empty);
                clientPage.ClientResponse.Insert(this.ID, "beforeEnd", str);
            }
            NameValueCollection form = (NameValueCollection)null;
            if (HttpContext.Current.Handler is System.Web.UI.Page handler)
                form = handler.Request.Form;
            if (form == null || !this.Validate(form))
                return;
            clientPage.ClientResponse.SetReturnValue(true);
        }

        /// <summary>Sets the modified flag.</summary>
        protected override void SetModified()
        {
            base.SetModified();
            if (!this.TrackModified)
                return;
            Sitecore.Context.ClientPage.Modified = true;
        }

        /// <summary>Builds the control.</summary>
        private void BuildControl()
        {
            if (!string.IsNullOrWhiteSpace(this.Value))
            {
                var controlObject = JsonConvert.DeserializeObject<JObject>(this.Value);
                foreach (var property in controlObject.Properties())
                {
                    this.Controls.Add((System.Web.UI.Control)new LiteralControl(this.BuildParameterKeyValue(property.Name, (string)property.Value)));
                }
            }
            
            this.Controls.Add((System.Web.UI.Control)new LiteralControl(this.BuildParameterKeyValue(string.Empty, string.Empty)));
        }

        /// <summary>Builds the parameter key value.</summary>
        /// <param name="key">The parameter key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The parameter key value.</returns>
        /// <contract><requires name="key" condition="not null" /><requires name="value" condition="not null" /><ensures condition="not null" /></contract>
        private string BuildParameterKeyValue(string key, string value)
        {
            Assert.ArgumentNotNull((object)key, nameof(key));
            Assert.ArgumentNotNull((object)value, nameof(value));
            string uniqueId = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID(this.ID + "_Param");
            Sitecore.Context.ClientPage.ServerProperties[this.ID + "_LastParameterID"] = (object)uniqueId;
            string clientEvent = Sitecore.Context.ClientPage.GetClientEvent(this.ID + ".ParameterChange");
            string str1 = this.ReadOnly ? " readonly=\"readonly\"" : string.Empty;
            string str2 = this.Disabled ? " disabled=\"disabled\"" : string.Empty;
            string str3 = this.IsVertical ? "</tr><tr>" : string.Empty;
            return string.Format("<table width=\"50%\" class='scAdditionalParameters'><tr><td width=\"20%\">{0}</td>{2}<td width=\"80%\">{1}</td></tr></table>",
                (object)this.GetValueHtmlControl(uniqueId, StringUtil.EscapeQuote(key)),
                (object)string.Format("<textarea  id=\"{0}_value\" name=\"{1}_value\" {2}{3} style=\"{6}\" onchange=\"{5}\"/>{4}</textarea>", (object)uniqueId, (object)uniqueId, (object)str1, (object)str2, (object)StringUtil.EscapeQuote(value), (object)clientEvent, (object)this.NameStyle), (object)str3);
        }

        /// <summary>Loads the post data.</summary>
        private void LoadValue()
        {
            if (this.ReadOnly || this.Disabled)
                return;
            NameValueCollection nameValueCollection = !(HttpContext.Current.Handler is System.Web.UI.Page handler) ? new NameValueCollection() : handler.Request.Form;
            var model = new JObject();
            foreach (string key1 in nameValueCollection.Keys)
            {
                if (!string.IsNullOrEmpty(key1) && key1.StartsWith(this.ID + "_Param", StringComparison.InvariantCulture) && !key1.EndsWith("_value", StringComparison.InvariantCulture))
                {
                    
                    string key = nameValueCollection[key1];
                    string value = nameValueCollection[key1 + "_value"];
                    if (!string.IsNullOrEmpty(value))
                    {
                        var jProperty = new JProperty(key, value);
                        model.Add(jProperty);
                    }
                }
            }
            string str1 = JsonConvert.SerializeObject(model);
            if (this.Value == str1)
                return;
            this.Value = str1;
            this.SetModified();
        }

        /// <summary>Validates the specified client page.</summary>
        /// <param name="form">The form.</param>
        /// <returns>The result of the validation.</returns>
        private bool Validate(NameValueCollection form)
        {
            Assert.ArgumentNotNull((object)form, nameof(form));
            return true;
        }
        
        /// <summary>Is control vertical</summary>
        protected virtual bool IsVertical => false;

        public string FieldName
        {
            get => this.GetViewStateString(nameof(FieldName));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(FieldName), value);
            }
        }

        /// <summary>Gets or sets the item ID.</summary>
        /// <value>The item ID.</value>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        ///   <ensures condition="nullable" />
        /// </contract>
        public string ItemID
        {
            get => this.GetViewStateString(nameof(ItemID));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(ItemID), value);
            }
        }

        /// <summary>Gets or sets the source.</summary>
        /// <value>The source.</value>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        ///   <ensures condition="nullable" />
        /// </contract>
        public string Source
        {
            get => this.GetViewStateString(nameof(Source));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(Source), value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Shell.Applications.ContentEditor.NameLookupValue" /> class.
        /// </summary>
        public OneWebNameValueList()
        {
            this.Activation = true;
            this.Class += " scCombobox";
        }

        /// <summary>Gets or sets the item language.</summary>
        /// <value>The item language.</value>
        public string ItemLanguage
        {
            get => StringUtil.GetString(this.ViewState[nameof(ItemLanguage)]);
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.ViewState[nameof(ItemLanguage)] = (object)value;
            }
        }

        /// <summary>Gets the items.</summary>
        /// <param name="current">The current.</param>
        /// <returns>The items.</returns>
        protected virtual Item[] GetItems(Item current)
        {
            Assert.ArgumentNotNull((object)current, nameof(current));
            using (new LanguageSwitcher(this.ItemLanguage))
                return LookupSources.GetItems(current, this.Source);
        }

        /// <summary>Gets the item header.</summary>
        /// <param name="item">The item.</param>
        /// <returns>The item header.</returns>
        /// <contract>
        ///   <requires name="item" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        protected virtual string GetItemHeader(Item item)
        {
            Assert.ArgumentNotNull((object)item, nameof(item));
            string str = StringUtil.GetString(this.FieldName);
            return !str.StartsWith("@", StringComparison.InvariantCulture) ? (str.Length <= 0 ? item.DisplayName : item[this.FieldName]) : item[str.Substring(1)];
        }

        /// <summary>Gets the item value.</summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <contract>
        ///   <requires name="item" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        protected virtual string GetItemValue(Item item)
        {
            Assert.ArgumentNotNull((object)item, nameof(item));
            return item.ID.ToString();
        }

        /// <summary>Determines whether the specified item is selected.</summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if the specified item is selected; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsSelected(Item item)
        {
            Assert.ArgumentNotNull((object)item, nameof(item));
            return this.Value == item.ID.ToString() || this.Value == item.Paths.LongID;
        }

        /// <summary>Gets value html control.</summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        /// <returns>The formatted value html control.</returns>
        protected string GetValueHtmlControl(string id, string value)
        {
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter((TextWriter)new StringWriter());
            Item[] items = this.GetItems(Sitecore.Context.ContentDatabase.GetItem(this.ItemID));
            htmlTextWriter.Write("<select id=\"" + id + "_\" name=\"" + id + "\"" + this.GetControlAttributes() + ">");
            htmlTextWriter.Write("<option" + (string.IsNullOrEmpty(value) ? " selected=\"selected\"" : string.Empty) + " value=\"\"></option>");
            foreach (Item obj in items)
            {
                string itemHeader = this.GetItemHeader(obj);
                bool flag = obj.ID.ToString() == value;
                htmlTextWriter.Write("<option value=\"" + this.GetItemValue(obj) + "\"" + (flag ? " selected=\"selected\"" : string.Empty) + ">" + itemHeader + "</option>");
            }
            htmlTextWriter.Write("</select>");
            return htmlTextWriter.InnerWriter.ToString();
        }

        /// <summary>Name html control style</summary>
        protected string NameStyle => "height: 70px; background-color:lightgrey'";
    }
}
