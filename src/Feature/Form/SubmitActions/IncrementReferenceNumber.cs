using System;
using OneWeb.Feature.Form.Constants;
using OneWeb.Feature.Form.Helpers;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;

namespace OneWeb.Feature.Form.SubmitActions
{
    public class IncrementReferenceNumber : SubmitActionBase<string>
    {
        public IncrementReferenceNumber(ISubmitActionData submitActionData) : base(submitActionData)
        {
                
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            try
            {
                var referenceNumber = FormHelper.FetchReferenceNumber(formSubmitContext, Constant.ReferenceNumberAccessType.IncrementAndLock);
                return true;
            }

            catch (FormatException ex)
            {
                LogManager.LogError("Submit Actions >> Increment Reference Number >> Error Message >> " + ex.Message, ex);
                return false;
            }
        }
    }
}