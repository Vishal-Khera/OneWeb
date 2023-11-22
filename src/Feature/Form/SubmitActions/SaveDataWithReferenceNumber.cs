using System.Linq;
using OneWeb.Feature.Form.Constants;
using OneWeb.Feature.Form.Helpers;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;

namespace OneWeb.Feature.Form.SubmitActions
{
    public class SaveDataWithReferenceNumber : SaveData
    {
        public SaveDataWithReferenceNumber(ISubmitActionData submitActionData) : base(submitActionData)
        {
            
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            var referenceNumberField = formSubmitContext.Fields.FirstOrDefault<IViewModel>(x => x.Name.ToLower() == Constant.FormContainer.ReferenceNumber);
            if (referenceNumberField != null)
            {
                var referenceNumber = FormHelper.FetchReferenceNumber(formSubmitContext, Constant.ReferenceNumberAccessType.ReadAndRelease);
                FormHelper.SetValue(formSubmitContext.Fields.FirstOrDefault<IViewModel>(x => x.Name.ToLower() == Constant.FormContainer.ReferenceNumber), referenceNumber);

                Sitecore.Diagnostics.Log.Error($"SaveDataWithReferenceNumber - {referenceNumber}", "Form Helper");
            }

            base.Execute(data, formSubmitContext);
            return true;
        }



    }
}