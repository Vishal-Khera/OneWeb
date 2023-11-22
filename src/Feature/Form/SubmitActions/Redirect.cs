// Decompiled with JetBrains decompiler
// Type: Sitecore.ExperienceForms.Processing.Actions.RedirectToPage
// Assembly: Sitecore.ExperienceForms, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A1E6B42A-5C54-47F7-AA35-9DBD85993A83
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.ExperienceForms.dll

using System;
using OneWeb.Feature.Form.Model;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.Links;
using Sitecore.Text;

namespace OneWeb.Feature.Form.SubmitActions
{
    public class Redirect : SubmitActionBase<RedirectModel>
    {
        public Redirect(ISubmitActionData submitActionData)
            : base(submitActionData)
        {
        }

        protected override bool Execute(RedirectModel data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull((object)formSubmitContext, nameof(formSubmitContext));
            if (data == null || (string.IsNullOrWhiteSpace(data.ExternalLink)) && !(data.InternalItemId != Guid.Empty))
                return false;

            string redirectUrl;
            if (!string.IsNullOrWhiteSpace(data.ExternalLink))
            {
                redirectUrl = data.ExternalLink;
            }
            else
            {
                Item obj = Context.Database.GetItem(new ID(data.InternalItemId));
                if (obj == null)
                    return false;

                UrlOptions defaultUrlOptions = LinkManager.GetDefaultUrlOptions();
                defaultUrlOptions.SiteResolving = Settings.Rendering.SiteResolving;
                redirectUrl = new UrlString(LinkManager.GetItemUrl(obj, defaultUrlOptions)).ToString();
            }

            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                if (!string.IsNullOrWhiteSpace(data.QueryString))
                {
                    redirectUrl += "?" + data.QueryString;
                }

                if (!string.IsNullOrWhiteSpace(data.Anchor))
                {
                    redirectUrl += "#" + data.Anchor;
                }

                formSubmitContext.RedirectUrl = redirectUrl;
                formSubmitContext.RedirectOnSuccess = true;
                formSubmitContext.Abort();
            }

            return true;
        }
    }
}