// Decompiled with JetBrains decompiler
// Type: Sitecore.Shell.Applications.ContentEditor.TreeList
// Assembly: Sitecore.Kernel, Version=13.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA8A253-0368-41B5-844C-99E9068D32BA
// Assembly location: C:\Projects\OneWeb\packages\Sitecore.Kernel.9.2.0\lib\net471\Sitecore.Kernel.dll
// XML documentation location: C:\Projects\OneWeb\packages\Sitecore.Kernel.9.2.0\lib\net471\Sitecore.Kernel.xml

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Classes.FilterQueryBuilders;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Resources;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Shell.Applications.ContentEditor.FieldHelpers;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.HtmlControls.Data;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;

namespace OneWeb.Foundation.SitecoreExtensions.Classes.Fields
{
    /// <summary>Summary description for TreeMultiList.</summary>
    public class OneWebTagTreelist : Sitecore.Web.UI.HtmlControls.Control, IContentField
    {
        private string _itemID;
        private Listbox _listBox;
        private string _source;
        /// <summary>
        /// The filter query builder that limits the number of elements to show.
        /// </summary>
        protected readonly OneWebTreeListFilterQueryBuilder FilterQueryBuilder;

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:OneWeb.Foundation.SitecoreExtensions.Classes.Fields.TreeList" /> class.
        /// </summary>
        public OneWebTagTreelist()
        {
            this.Class = "scContentControl scContentControlTreelist";
            this.Activation = true;
            this.ReadOnly = false;
            this.FilterQueryBuilder = new OneWebTreeListFilterQueryBuilder();
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the <see cref="T:OneWeb.Foundation.SitecoreExtensions.Classes.Fields.TreeList" /> allows the multiple selection.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the <see cref="T:OneWeb.Foundation.SitecoreExtensions.Classes.Fields.TreeList" /> allows the  multiple selection; otherwise, <c>false</c>.
        /// </value>
        [Category("Data")]
        [Description("If set to Yes, allows the same item to be selected more than once")]
        public bool AllowMultipleSelection
        {
            get => this.GetViewStateBool(nameof(AllowMultipleSelection));
            set => this.SetViewStateBool(nameof(AllowMultipleSelection), value);
        }

        /// <summary>Gets the data Sitecore.Context parameters.</summary>
        /// <value>The data Sitecore.Context parameters.</value>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        public string DatabaseName
        {
            get => this.GetViewStateString(nameof(DatabaseName));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(DatabaseName), value);
            }
        }

        /// <summary>
        ///   Gets or sets field that will be used as source for ListItem header. If empty- DisplayName will be used.
        /// </summary>
        public string DisplayFieldName
        {
            get => this.GetViewStateString(nameof(DisplayFieldName));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(DisplayFieldName), value);
            }
        }

        /// <summary>Gets or sets the exclude items for display.</summary>
        /// <value>The exclude items for display.</value>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        [Category("Data")]
        [Description("Comma separated list of item names/ids.")]
        public string ExcludeItemsForDisplay
        {
            get => this.GetViewStateString(nameof(ExcludeItemsForDisplay));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(ExcludeItemsForDisplay), value);
            }
        }

        /// <summary>Gets or sets the exclude templates for display.</summary>
        /// <value>The exclude templates for display.</value>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        [Category("Data")]
        [Description("Comma separated list of template names. If this value is set, items based on these template will not be displayed in the tree.")]
        public string ExcludeTemplatesForDisplay
        {
            get => this.GetViewStateString(nameof(ExcludeTemplatesForDisplay));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(ExcludeTemplatesForDisplay), value);
            }
        }

        /// <summary>Gets or sets the exclude templates for selection.</summary>
        /// <value>The exclude templates for selection.</value>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        [Category("Data")]
        [Description("Comma separated list of template names. If this value is set, items based on these template will not be included in the menu.")]
        public string ExcludeTemplatesForSelection
        {
            get => this.GetViewStateString(nameof(ExcludeTemplatesForSelection));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(ExcludeTemplatesForSelection), value);
            }
        }

        /// <summary>Gets or sets the include items for display.</summary>
        /// <value>The include items for display.</value>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        [Category("Data")]
        [Description("Comma separated list of items names/ids.")]
        public string IncludeItemsForDisplay
        {
            get => this.GetViewStateString(nameof(IncludeItemsForDisplay));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(IncludeItemsForDisplay), value);
            }
        }

        /// <summary>Gets or sets the include templates for display.</summary>
        /// <value>The include templates for display.</value>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        [Category("Data")]
        [Description("Comma separated list of template names. If this value is set, only items based on these template can be displayed in the menu.")]
        public string IncludeTemplatesForDisplay
        {
            get => this.GetViewStateString(nameof(IncludeTemplatesForDisplay));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(IncludeTemplatesForDisplay), value);
            }
        }

        /// <summary>Gets or sets the include templates for selection.</summary>
        /// <value>The include templates for selection.</value>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        [Category("Data")]
        [Description("Comma separated list of template names. If this value is set, only items based on these template can be included in the menu.")]
        public string IncludeTemplatesForSelection
        {
            get => this.GetViewStateString(nameof(IncludeTemplatesForSelection));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(IncludeTemplatesForSelection), value);
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
            get => this._itemID;
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this._itemID = value;
            }
        }

        /// <summary>Gets or sets the item language.</summary>
        /// <value>The item language.</value>
        public string ItemLanguage
        {
            get => this.GetViewStateString(nameof(ItemLanguage));
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.SetViewStateString(nameof(ItemLanguage), value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if  this instance is read only; otherwise, <c>false</c>.</value>
        public virtual bool ReadOnly
        {
            get => this.GetViewStateBool(nameof(ReadOnly));
            set
            {
                this.SetViewStateBool(nameof(ReadOnly), value);
                if (value)
                {
                    this.Attributes["readonly"] = "readonly";
                    this.Disabled = true;
                }
                else
                    this.Attributes.Remove("readonly");
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
            get => this._source;
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this._source = value;
            }
        }

        /// <summary>Raises the load event.</summary>
        /// <param name="args">The arguments.</param>
        /// <contract>
        ///   <requires name="args" condition="not null" />
        /// </contract>
        protected override void OnLoad(EventArgs args)
        {
            Assert.ArgumentNotNull((object)args, nameof(args));
            if (!Sitecore.Context.ClientPage.IsEvent)
            {
                this.SetProperties();
                Border child1 = new Border();
                this.Controls.Add((System.Web.UI.Control)child1);
                this.GetControlAttributes();
                foreach (string key in (IEnumerable)this.Attributes.Keys)
                    child1.Attributes.Add(key, this.Attributes[key]);
                child1.Attributes["id"] = this.ID;
                Border border1 = new Border();
                border1.Class = "scTreeListHalfPart";
                Border child2 = border1;
                child1.Controls.Add((System.Web.UI.Control)child2);
                Border child3 = new Border();
                child2.Controls.Add((System.Web.UI.Control)child3);
                this.SetViewStateString("ID", this.ID);
                ControlCollection controls1 = child3.Controls;
                Literal child4 = new Literal("All");
                child4.Class = "scContentControlMultilistCaption";
                controls1.Add((System.Web.UI.Control)child4);
                Scrollbox scrollbox = new Scrollbox();
                scrollbox.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("S");
                scrollbox.Class = "scScrollbox scContentControlTree";
                Scrollbox child5 = scrollbox;
                child3.Controls.Add((System.Web.UI.Control)child5);
                TreeviewEx treeviewEx = new TreeviewEx();
                treeviewEx.ID = this.ID + "_all";
                treeviewEx.DblClick = this.ID + ".Add";
                treeviewEx.AllowDragging = false;
                TreeviewEx child6 = treeviewEx;
                child5.Controls.Add((System.Web.UI.Control)child6);
                Border border2 = new Border();
                border2.Class = "scContentControlNavigation";
                Border child7 = border2;
                child2.Controls.Add((System.Web.UI.Control)child7);
                ImageBuilder imageBuilder1 = new ImageBuilder()
                {
                    Src = "Office/16x16/navigate_right.png",
                    Class = "scNavButton",
                    ID = this.ID + "_right",
                    OnClick = Sitecore.Context.ClientPage.GetClientEvent(this.ID + ".Add")
                };
                ImageBuilder imageBuilder2 = new ImageBuilder()
                {
                    Src = "Office/16x16/navigate_left.png",
                    Class = "scNavButton",
                    ID = this.ID + "_left",
                    OnClick = Sitecore.Context.ClientPage.GetClientEvent(this.ID + ".Remove")
                };
                LiteralControl child8 = new LiteralControl(imageBuilder1.ToString() + (object)imageBuilder2);
                child7.Controls.Add((System.Web.UI.Control)child8);
                Border border3 = new Border();
                border3.Class = "scTreeListHalfPart";
                Border child9 = border3;
                child1.Controls.Add((System.Web.UI.Control)child9);
                Border border4 = new Border();
                border4.Class = "scFlexColumnContainerWithoutFlexie";
                Border child10 = border4;
                child9.Controls.Add((System.Web.UI.Control)child10);
                ControlCollection controls2 = child10.Controls;
                Literal child11 = new Literal("Selected");
                child11.Class = "scContentControlMultilistCaption";
                controls2.Add((System.Web.UI.Control)child11);
                Border border5 = new Border();
                border5.Class = "scContentControlSelectedList";
                Border child12 = border5;
                child10.Controls.Add((System.Web.UI.Control)child12);
                Listbox child13 = new Listbox();
                child12.Controls.Add((System.Web.UI.Control)child13);
                this._listBox = child13;
                child13.ID = this.ID + "_selected";
                child13.DblClick = this.ID + ".Remove";
                child13.Style["width"] = "100%";
                child13.Size = "10";
                child13.Attributes["onchange"] = "javascript:document.getElementById('" + this.ID + "_help').innerHTML=this.selectedIndex>=0?this.options[this.selectedIndex].innerHTML:''";
                child13.Attributes["class"] = "scContentControlMultilistBox scFlexContentWithoutFlexie";
                this._listBox.TrackModified = false;
                child6.Enabled = !this.ReadOnly;
                child13.Disabled = this.ReadOnly;
                child10.Controls.Add((System.Web.UI.Control)new LiteralControl("<div class='scContentControlTreeListHelp' id=\"" + this.ID + "_help\"></div>"));
                Border border6 = new Border();
                border6.Class = "scContentControlNavigation";
                Border child14 = border6;
                child9.Controls.Add((System.Web.UI.Control)child14);
                ImageBuilder imageBuilder3 = new ImageBuilder()
                {
                    Src = "Office/16x16/navigate_up.png",
                    Class = "scNavButton",
                    ID = this.ID + "_up",
                    OnClick = Sitecore.Context.ClientPage.GetClientEvent(this.ID + ".Up")
                };
                ImageBuilder imageBuilder4 = new ImageBuilder()
                {
                    Src = "Office/16x16/navigate_down.png",
                    Class = "scNavButton",
                    ID = this.ID + "_down",
                    OnClick = Sitecore.Context.ClientPage.GetClientEvent(this.ID + ".Down")
                };
                LiteralControl child15 = new LiteralControl(imageBuilder3.ToString() + (object)imageBuilder4);
                child14.Controls.Add((System.Web.UI.Control)child15);
                DataContext child16 = new DataContext();
                child1.Controls.Add((System.Web.UI.Control)child16);
                child16.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("D");
                child16.Filter = this.FormTemplateFilterForDisplay();
                child6.DataContext = child16.ID;
                child6.DisplayFieldName = this.DisplayFieldName;
                child16.DataViewName = "Master";
                if (!string.IsNullOrEmpty(this.DatabaseName))
                    child16.Parameters = "databasename=" + this.DatabaseName;
                child16.Root = this.DataSource;
                child16.Language = Language.Parse(this.ItemLanguage);
                child6.ShowRoot = true;
                this.RestoreState();
            }
            base.OnLoad(args);
        }

        /// <summary>Adds data.</summary>
        protected void Add()
        {
            if (this.Disabled)
                return;
            string viewStateString = this.GetViewStateString("ID");
            TreeviewEx control1 = this.FindControl(viewStateString + "_all") as TreeviewEx;
            Assert.IsNotNull((object)control1, typeof(DataTreeview));
            Listbox control2 = this.FindControl(viewStateString + "_selected") as Listbox;
            Assert.IsNotNull((object)control2, typeof(Listbox));
            Item selectionItem = control1.GetSelectionItem(Language.Parse(this.ItemLanguage), Sitecore.Data.Version.Latest);
            if (selectionItem == null)
            {
                SheerResponse.Alert("Select an item in the Content Tree.");
            }
            else
            {
                if (this.HasExcludeTemplateForSelection(selectionItem))
                    return;
                if (this.IsDeniedMultipleSelection(selectionItem, control2))
                {
                    SheerResponse.Alert("You cannot select the same item twice.");
                }
                else
                {
                    if (!this.HasIncludeTemplateForSelection(selectionItem))
                        return;
                    SheerResponse.Eval("scForm.browser.getControl('" + viewStateString + "_selected').selectedIndex=-1");
                    ListItem listItem = new ListItem();
                    listItem.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("L");
                    Sitecore.Context.ClientPage.AddControl((System.Web.UI.Control)control2, (System.Web.UI.Control)listItem);
                    listItem.Header = this.GetHeaderValue(selectionItem) + (string.IsNullOrWhiteSpace(GetParentTagCategory(selectionItem)) ? string.Empty : $" <{GetParentTagCategory(selectionItem)}>");
                    listItem.Value = listItem.ID + "|" + (object)selectionItem.ID;
                    SheerResponse.Refresh((Sitecore.Web.UI.HtmlControls.Control)control2);
                    OneWebTagTreelist.SetModified();
                }
            }
        }

        /// <summary>Executes the Down event.</summary>
        protected void Down()
        {
            if (this.Disabled)
                return;
            Listbox control1 = this.FindControl(this.GetViewStateString("ID") + "_selected") as Listbox;
            Assert.IsNotNull((object)control1, typeof(Listbox));
            int num = -1;
            for (int index = control1.Controls.Count - 1; index >= 0; --index)
            {
                ListItem control2 = control1.Controls[index] as ListItem;
                Assert.IsNotNull((object)control2, typeof(ListItem));
                if (!control2.Selected)
                {
                    num = index - 1;
                    break;
                }
            }
            for (int index = num; index >= 0; --index)
            {
                ListItem control3 = control1.Controls[index] as ListItem;
                Assert.IsNotNull((object)control3, typeof(ListItem));
                if (control3.Selected)
                {
                    SheerResponse.Eval("scForm.browser.swapNode(scForm.browser.getControl('" + control3.ID + "'), scForm.browser.getControl('" + control3.ID + "').nextSibling);");
                    control1.Controls.Remove((System.Web.UI.Control)control3);
                    control1.Controls.AddAt(index + 1, (System.Web.UI.Control)control3);
                }
            }
            OneWebTagTreelist.SetModified();
        }

        /// <summary>Gets the header value.</summary>
        /// <param name="item">The item.</param>
        /// <returns>Header text for list item.</returns>
        protected virtual string GetHeaderValue(Item item)
        {
            Assert.ArgumentNotNull((object)item, nameof(item));
            string str = string.IsNullOrEmpty(this.DisplayFieldName) ? item.DisplayName : item[this.DisplayFieldName];
            return string.IsNullOrEmpty(str) ? item.DisplayName : str;
        }

        /// <summary>Removes the selected item.</summary>
        protected void Remove()
        {
            if (this.Disabled)
                return;
            string viewStateString = this.GetViewStateString("ID");
            Listbox control = this.FindControl(viewStateString + "_selected") as Listbox;
            Assert.IsNotNull((object)control, typeof(Listbox));
            SheerResponse.Eval("scForm.browser.getControl('" + viewStateString + "_all').selectedIndex=-1");
            SheerResponse.Eval("scForm.browser.getControl('" + viewStateString + "_help').innerHTML=''");
            foreach (ListItem listItem in control.Selected)
            {
                SheerResponse.Remove(listItem.ID);
                control.Controls.Remove((System.Web.UI.Control)listItem);
            }
            SheerResponse.Refresh((Sitecore.Web.UI.HtmlControls.Control)control);
            OneWebTagTreelist.SetModified();
        }

        /// <summary>Moves the selected items up.</summary>
        protected void Up()
        {
            if (this.Disabled)
                return;
            Listbox control = this.FindControl(this.GetViewStateString("ID") + "_selected") as Listbox;
            Assert.IsNotNull((object)control, typeof(Listbox));
            ListItem selectedItem = control.SelectedItem;
            if (selectedItem == null)
                return;
            int num = control.Controls.IndexOf((System.Web.UI.Control)selectedItem);
            if (num == 0)
                return;
            SheerResponse.Eval("scForm.browser.swapNode(scForm.browser.getControl('" + selectedItem.ID + "'), scForm.browser.getControl('" + selectedItem.ID + "').previousSibling);");
            control.Controls.Remove((System.Web.UI.Control)selectedItem);
            control.Controls.AddAt(num - 1, (System.Web.UI.Control)selectedItem);
            OneWebTagTreelist.SetModified();
        }

        /// <summary>Gets the value.</summary>
        /// <returns>The value of the field.</returns>
        /// <contract>
        ///   <ensures condition="not null" />
        /// </contract>
        public string GetValue()
        {
            ListString listString = new ListString();
            Listbox control1 = this.FindControl(this.GetViewStateString("ID") + "_selected") as Listbox;
            Assert.IsNotNull((object)control1, typeof(Listbox));
            foreach (Sitecore.Web.UI.HtmlControls.Control control2 in control1.Items)
            {
                string[] strArray = control2.Value.Split('|');
                if (strArray.Length > 1)
                    listString.Add(strArray[1]);
            }
            return listString.ToString();
        }

        /// <summary>Sets the value.</summary>
        /// <param name="text">The text.</param>
        /// <contract>
        ///   <requires name="text" condition="not null" />
        /// </contract>
        public void SetValue(string text)
        {
            Assert.ArgumentNotNull((object)text, nameof(text));
            this.Value = text;
        }

        /// <summary>Sets the modified.</summary>
        protected static void SetModified() => Sitecore.Context.ClientPage.Modified = true;

        /// <summary>
        ///   Determines whether an item is based on a template from <paramref name="templateList" />.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="templateList">The template list - a set of comma-separated template names.</param>
        /// <returns>
        ///   <c>true</c> if item is based on a template, which name is mentioned in <paramref name="templateList" />; otherwise, <c>false</c>.
        /// </returns>
        /// <contract>
        ///   <requires name="item" condition="none" />
        ///   <requires name="templateList" condition="not null" />
        /// </contract>
        private bool HasItemTemplate(Item item, string templateList)
        {
            Assert.ArgumentNotNull((object)templateList, nameof(templateList));
            if (item == null || templateList.Length == 0)
                return false;
            string[] strArray = templateList.Split(',');
            ArrayList arrayList = new ArrayList(strArray.Length);
            for (int index = 0; index < strArray.Length; ++index)
                arrayList.Add((object)strArray[index].Trim().ToLowerInvariant());
            return arrayList.Contains((object)item.TemplateName.Trim().ToLowerInvariant());
        }

        /// <summary>
        ///   Can be used after <c>OnLoad()</c> is called.
        ///   Fulfills parsing Of Value and restores <c>Listbox</c> state.
        /// </summary>
        /// <returns></returns>
        /// <contract>
        ///   <ensures condition="not null" />
        /// </contract>
        protected virtual string FormTemplateFilterForDisplay()
        {
            return this.FilterQueryBuilder.BuildFilterQuery(this);
        }

        /// <summary>
        ///   Determines whether an item is based on a template that is mentioned in <see cref="P:OneWeb.Foundation.SitecoreExtensions.Classes.Fields.TreeList.ExcludeTemplatesForSelection" />.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if item is based on a template that is mentioned in <see cref="P:OneWeb.Foundation.SitecoreExtensions.Classes.Fields.TreeList.ExcludeTemplatesForSelection" />; otherwise, <c>false</c>.
        /// </returns>
        /// <contract>
        ///   <requires name="item" condition="none" />
        /// </contract>
        private bool HasExcludeTemplateForSelection(Item item) => item == null || this.HasItemTemplate(item, this.ExcludeTemplatesForSelection);

        /// <summary>
        ///   Determines whether [has include template for selection] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [has include template for selection] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        /// <contract>
        ///   <requires name="item" condition="not null" />
        /// </contract>
        private bool HasIncludeTemplateForSelection(Item item)
        {
            Assert.ArgumentNotNull((object)item, nameof(item));
            return this.IncludeTemplatesForSelection.Length == 0 || this.HasItemTemplate(item, this.IncludeTemplatesForSelection);
        }

        /// <summary>
        ///   Determines whether this instance denies multiple selection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="listbox">The <c>listbox</c>.</param>
        /// <returns>
        ///   <c>true</c> if this instance denies multiple selection; otherwise, <c>false</c>.
        /// </returns>
        private bool IsDeniedMultipleSelection(Item item, Listbox listbox)
        {
            Assert.ArgumentNotNull((object)listbox, nameof(listbox));
            if (item == null)
                return true;
            if (this.AllowMultipleSelection)
                return false;
            foreach (Sitecore.Web.UI.HtmlControls.Control control in listbox.Controls)
            {
                string[] strArray = control.Value.Split('|');
                if (strArray.Length >= 2 && strArray[1] == item.ID.ToString())
                    return true;
            }
            return false;
        }

        /// <summary>Restores the state.</summary>
        private void RestoreState()
        {
            string[] strArray = this.Value.Split('|');
            if (strArray.Length == 0)
                return;
            Database database = Sitecore.Context.ContentDatabase;
            if (!string.IsNullOrEmpty(this.DatabaseName))
                database = Factory.GetDatabase(this.DatabaseName);
            for (int index = 0; index < strArray.Length; ++index)
            {
                string path = strArray[index];
                if (!string.IsNullOrEmpty(path))
                {
                    ListItem listItem = new ListItem();
                    listItem.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("I");
                    ListItem child = listItem;
                    this._listBox.Controls.Add((System.Web.UI.Control)child);
                    child.Value = child.ID + "|" + path;
                    Item obj = database.GetItem(path, Language.Parse(this.ItemLanguage));
                    child.Header = obj == null ? path + " " + Translate.Text("[Item not found]") : this.GetHeaderValue(obj) + (string.IsNullOrWhiteSpace(GetParentTagCategory(obj)) ? string.Empty : $" <{GetParentTagCategory(obj)}>");
                }
            }
            SheerResponse.Refresh((Sitecore.Web.UI.HtmlControls.Control)this._listBox);
        }

        /// <summary>Sets the properties.</summary>
        private void SetProperties()
        {
            string str = StringUtil.GetString(this.Source);
            if (str.StartsWith("query:"))
            {
                if (Sitecore.Context.ContentDatabase == null || this.ItemID == null)
                    return;
                Item current = Sitecore.Context.ContentDatabase.GetItem(this.ItemID);
                if (current == null)
                    return;
                Item obj = (Item)null;
                try
                {
                    obj = ((IEnumerable<Item>)LookupSources.GetItems(current, str)).FirstOrDefault<Item>();
                }
                catch (Exception ex)
                {
                    Log.Error("Treelist field failed to execute query.", ex, (object)this);
                }
                if (obj == null)
                    return;
                this.DataSource = obj.Paths.FullPath;
            }
            else if (Sitecore.Data.ID.IsID(str))
                this.DataSource = this.Source;
            else if (this.Source != null && !str.Trim().StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                this.ExcludeTemplatesForSelection = StringUtil.ExtractParameter("ExcludeTemplatesForSelection", this.Source).Trim();
                this.IncludeTemplatesForSelection = StringUtil.ExtractParameter("IncludeTemplatesForSelection", this.Source).Trim();
                this.IncludeTemplatesForDisplay = StringUtil.ExtractParameter("IncludeTemplatesForDisplay", this.Source).Trim();
                this.ExcludeTemplatesForDisplay = StringUtil.ExtractParameter("ExcludeTemplatesForDisplay", this.Source).Trim();
                this.ExcludeItemsForDisplay = StringUtil.ExtractParameter("ExcludeItemsForDisplay", this.Source).Trim();
                this.IncludeItemsForDisplay = StringUtil.ExtractParameter("IncludeItemsForDisplay", this.Source).Trim();
                this.AllowMultipleSelection = string.Compare(StringUtil.ExtractParameter("AllowMultipleSelection", this.Source).Trim().ToLowerInvariant(), "yes", StringComparison.InvariantCultureIgnoreCase) == 0;
                this.DataSource = StringUtil.ExtractParameter("DataSource", this.Source).Trim().ToLowerInvariant();
                this.DatabaseName = StringUtil.ExtractParameter("databasename", this.Source).Trim().ToLowerInvariant();
            }
            else
                this.DataSource = this.Source;
        }

        private string GetParentTagCategory(Item item)
        {
            if (item != null && item.Parent != null &&
                StringExtensions.IdEqualsGuid(item.Parent.TemplateID, OneWebTagCategory.TemplateIdString))
            {
                return this.GetHeaderValue(item.Parent);
            }

            return string.Empty;
        }
    }
}
