using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Feature.FormsExtensions.Fields.FileUpload;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneWeb.Feature.Form.Constants;
using OneWeb.Feature.Form.Helpers;
using OneWeb.Feature.Form.Model;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;

namespace OneWeb.Feature.Form.SubmitActions
{
    public class SendEmail : SubmitActionBase<string>
    {
        private EmailModel _mailModel;

        public SendEmail(ISubmitActionData submitActionData) : base(submitActionData)
        {
            //added logs till SMTP changes           
        }

        protected override bool TryParse(string value, out string target)
        {
            if (!string.IsNullOrEmpty(value))
                _mailModel = JsonConvert.DeserializeObject<EmailModel>(value, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    Error = delegate (object sender, ErrorEventArgs earg)
                    {
                        LogManager.LogInfo("SendEmail >> Error in TryParse - " + Convert.ToString(earg.ErrorContext.Member));
                        earg.ErrorContext.Handled = true;
                    }
                });
            target = string.Empty;
            return true;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            var from = string.Empty;
            var to = string.Empty;
            try
            {
                var referenceNumberField = formSubmitContext.Fields.FirstOrDefault<IViewModel>(x => x.Name.ToLower() == Constant.FormContainer.ReferenceNumber);
                if (referenceNumberField != null)
                {
                    var referenceNumber = FormHelper.FetchReferenceNumber(formSubmitContext, Constant.ReferenceNumberAccessType.Read);
                    FormHelper.SetValue(formSubmitContext.Fields.FirstOrDefault<IViewModel>(x => x.Name.ToLower() == Constant.FormContainer.ReferenceNumber), referenceNumber);

                    Log.Error($"SendEmail - {referenceNumber}", "Form Helper");
                }
                from = ReplaceTokens(_mailModel.From, formSubmitContext.Fields); ;
                to = ReplaceTokens(_mailModel.To, formSubmitContext.Fields); //addressValue1
                var cc = ReplaceTokens(_mailModel.Cc, formSubmitContext.Fields); //addressValue2
                var bcc = ReplaceTokens(_mailModel.Bcc, formSubmitContext.Fields); //addressValue3
                var subject = ReplaceTokens(_mailModel.Subject, formSubmitContext.Fields); //str1
                var emailBody = ReplaceTokens(_mailModel.Message, formSubmitContext.Fields); //str2
                if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(from))
                {
                    return false;
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(from),
                    Subject = !string.IsNullOrWhiteSpace(subject) ? subject : string.Empty,
                    Body = emailBody,
                    IsBodyHtml = _mailModel.IsHtml
                };

                if (!AddEmailAddresses(to, "To", mailMessage))
                {
                    return false;
                }
                var attachments = GetAttachments(formSubmitContext);
                AddEmailAddresses(cc, Constant.LogsProperties.CC, mailMessage);
                AddEmailAddresses(bcc, Constant.LogsProperties.Bcc, mailMessage);
                //Code cleanup - remove db hit SMTP
                EmailExtensions.SendEmail(from, to, cc, bcc, subject, emailBody, string.Empty, attachments);
                return true;
            }

            catch (FormatException ex)
            {
                LogManager.LogError(
                    "MailExtensions >> Error Sending Mail >> From: " + from + ", To: " + to + " >> Error Message >> " +
                    ex.Message, ex);
                return false;
            }
            catch (Exception ex)
            {
                LogManager.LogError("Send SendEmail Action Error: " + ex.Message, ex);
                return false;
            }
        }

        protected bool AddEmailAddresses(
            string addressValue,
            string fieldName,
            MailMessage mailMessage)
        {
            if (string.IsNullOrWhiteSpace(addressValue) || string.IsNullOrWhiteSpace(fieldName) || mailMessage == null)
            {
                return false;
            }
            var strArray = addressValue.Split(new char[2] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            var property = mailMessage.GetType().GetProperty(fieldName);
            var addressCollection =
                ((object)property != null ? property.GetValue(mailMessage) : null) as MailAddressCollection;
            if (strArray.Length == 0 || addressCollection == null)
            {
                return false;
            }

            foreach (var address in strArray)
                try
                {
                    var mailAddress = new MailAddress(address);
                    addressCollection.Add(mailAddress);
                }
                catch
                {
                    LogManager.LogWarning(string.Format(
                        "Send SendEmail Action Warning: {0} in the {1} field is not a valid email address.", address,
                        fieldName.ToUpperInvariant()));
                }

            return addressCollection.Count > 0;
        }

        protected string ReplaceTokens(string template, IList<IViewModel> formFields)
        {
            if (string.IsNullOrWhiteSpace(template))
                return string.Empty;
            var matchCollection = Regex.Matches(template, "\\[(.+?)\\]");
            if (matchCollection.Count <= 0)
                return template;
            var dictionary = new Dictionary<string, bool>();

            foreach (Match match in matchCollection)
                if (!dictionary.ContainsKey(match.Value))
                {
                    var tokenName = match.Value.TrimStart('[').TrimEnd(']');

                    var iviewModel = formFields.FirstOrDefault(f => f.Name == tokenName);
                    if (iviewModel != null)
                    {
                        template = template.Replace(match.Value, GetFieldStringValue(iviewModel));
                    }
                    else
                    {
                        template = template.Replace(match.Value, string.Empty);
                        LogManager.LogWarning(string.Format("Field {0} not found in form, replacing by empty string.",
                            match.Value));
                    }

                    dictionary.Add(match.Value, true);
                }

            return template;
        }

        protected string GetFieldStringValue(object field)
        {
            if (field != null && field is ListViewModel)
            {
                var listViewModel = (ListViewModel)field;
                return listViewModel.Value == null || !listViewModel.Value.Any()
                    ? string.Empty
                    : string.Join(", ", listViewModel.Value);
            }

            string str;
            if (field == null)
            {
                str = string.Empty;
                return str;
            }
            
            if (field is CheckBoxViewModel)
            {
                var chkBx = (CheckBoxViewModel)field;
                str = chkBx.Value ? "Yes" :  "No";
                return str;
            }

            var property = field.GetType().GetProperty("Value");
            str = property is object ? property.GetValue(field, null)?.ToString() : string.Empty;
            return str ?? string.Empty;
        }
        private List<Attachment> GetAttachments(FormSubmitContext formSubmitContext)
        {
            if (formSubmitContext?.Fields == null)
            {
                return null;
            }
            var attachmentsList = new List<Attachment>();
            foreach (IViewModel field in formSubmitContext.Fields)
            {
                FileUploadModel fileUploadModel = field as FileUploadModel;
                if (fileUploadModel != null)
                {
                    if (fileUploadModel.File != null)
                    {

                        fileUploadModel.File.InputStream.Position = 0;

                        var attachment =
                            new Attachment(fileUploadModel.File?.InputStream, fileUploadModel.File.FileName);
                        attachmentsList.Add(attachment);

                    }
                }
            }

            return attachmentsList;
        }
    }
}