using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Microsoft.Ajax.Utilities;
using OneWeb.Foundation.SitecoreExtensions.Caching;
using OneWeb.Foundation.SitecoreExtensions.Logging;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public class EmailExtensions
    {
        private static SmtpClient GetSmtpFromConfig()
        {
            SmtpClient smtpClient;
            try
            {
                string strSmtpServer = Sitecore.Configuration.Settings.GetSetting("OneWebMailServer");
                string strPortNumber = Sitecore.Configuration.Settings.GetSetting("OneWebMailServerPort");
                string clientUsername = Sitecore.Configuration.Settings.GetSetting("OneWebMailServerUserName");
                string clientPassword = Sitecore.Configuration.Settings.GetSetting("OneWebMailServerPassword");
                if (string.IsNullOrEmpty(strSmtpServer))
                {
                    return null;
                }

                smtpClient = new SmtpClient(strSmtpServer);
                if (!string.IsNullOrEmpty(strPortNumber))
                {
                    smtpClient.Port = Convert.ToInt32(strPortNumber);
                }
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;

                if (!string.IsNullOrEmpty(clientUsername) && (!string.IsNullOrEmpty(clientPassword)))
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential(clientUsername, clientPassword);
                }
                return smtpClient;
            }
            catch (Exception ex)
            {
                LogManager.LogError("Error occured in GetSMTPFromConfig() method in Mail.cs file while creating SmtpClient object from config:" + ex.StackTrace, ex);
                return null;
            }
        }

        public static void SendEmail(string strFromEmail, string strToEmail, string strCcEmail, string stringBccEmail, string strSubject, string strBodyHtmlText, string mainSendingFeatureName = "", IList<Attachment> attachments = null)
        {
            bool mailsuccessfullydelivered = false;
            int i = 0;
            while (!mailsuccessfullydelivered)
            {
                if (i > 2)
                {
                    LogManager.LogWarning("Email cannot be delivered");
                    break;
                }

                MailMessage mail;
                using (SmtpClient smtpClient = CacheManager.GetOrSet("GetSmtpFromConfig", GetSmtpFromConfig, 86400))
                {
                    if (smtpClient == null)
                    {
                        LogManager.LogWarning("Error occured in SendEmail() method of Mail.cs." + mainSendingFeatureName + ": SmtpClient object cannot be created.");
                    }
                    else
                    {
                        try
                        {
                            mail = GetMailInMailMessageFormat(strFromEmail, strToEmail, strCcEmail, stringBccEmail, strSubject, strBodyHtmlText);
                            attachments.ForEach(x => mail.Attachments.Add(x));
                            mailsuccessfullydelivered = SendMail(mail, smtpClient, mainSendingFeatureName);
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogError("Error occured in Send(Mail) with sitecore credentials " + ex.StackTrace, ex);
                        }
                    }
                }
                i++;
            }
        }

        private static bool SendMail(MailMessage mail, SmtpClient smtpClient, string mainSendingFeatureName = "")
        {
            try
            {
                if (mail == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(mainSendingFeatureName))
                {
                    smtpClient.Send(mail);
                    return true;
                }
                else
                {
                    LogManager.LogDebug("Initiating Send Mail for " + mainSendingFeatureName);
                    smtpClient.Send(mail);
                    LogManager.LogDebug("Mail Sent Successfully for " + mainSendingFeatureName + " Email > " + mail.To.Select(x=> x.Address));
                    return true;

                }
            }
            catch (Exception ex)
            {

                if (!string.IsNullOrEmpty(mainSendingFeatureName))
                {
                    if (smtpClient == null)
                    {
                        LogManager.LogError("Error occured in smtpClient.Send(mail): Mail Delivery failed- " + mainSendingFeatureName + " SmtpClient object is null" + ex.Message + ".", ex);
                    }
                    else
                    {
                        if (mail != null)
                        {
                            LogManager.LogError("Error occured in smtpClient.Send(mail): Mail Delivery failed- " + mainSendingFeatureName + ". Following are the mail Details:- SMTPHost- " + smtpClient.Host + ", Port- " + smtpClient.Port + ".  Email Address:- " + mail.To.Select(x => x.Address) +" >> "+ ex.Message + ".", ex);
                        }
                    }
                }
                else
                {
                    LogManager.LogError("Error occured in smtpClient.Send(mail): Mail Delivery failed- " + mainSendingFeatureName + " " + ex.Message + ".", ex);
                }

                return false;
            }
        }

        private static MailMessage GetMailInMailMessageFormat(string strFromEmail, string strToEmail, string strCcEmail, string stringBccEmail, string strSubject, string strBodyHtmlText)
        {
            MailMessage mail = new MailMessage();
            try
            {
                var frmMail = strFromEmail.Split(',').FirstOrDefault();
                if (IsValidEmail(frmMail))
                {
                    mail.From = new  MailAddress(frmMail);
                }
                else
                {
                    throw new Exception(frmMail + " Invalid From Email. Admin email cannot be fetched. Please make sure that the configuration item for AdminEmail exists in Sitecore and has proper value.");
                }

                foreach (string mailAddress in strToEmail.Split(','))
                {
                    if (IsValidEmail(mailAddress.Trim()))
                    {
                        mail.To.Add(mailAddress.Trim());
                    }
                    else
                    {
                        throw new Exception(mailAddress + " is not a valid email id");
                    }
                }
                
                if (!string.IsNullOrEmpty(strCcEmail))
                {
                    foreach (string mailAddress in strCcEmail.Split(','))
                    {
                        if (IsValidEmail(mailAddress.Trim()))
                        {
                            mail.CC.Add(mailAddress.Trim());
                        }
                        else
                        {
                            throw new Exception(mailAddress + " is not a valid email id");
                        }
                    }
                }
                if (!string.IsNullOrEmpty(stringBccEmail))
                {
                    foreach (string mailAddress in stringBccEmail.Split(','))
                    {
                        if (IsValidEmail(mailAddress.Trim()))
                        {
                            mail.Bcc.Add(mailAddress.Trim());
                        }
                        else
                        {
                            throw new Exception(mailAddress + " is not a valid email id");
                        }
                    }
                }

                mail.Subject = strSubject;
                mail.Body = strBodyHtmlText;
                mail.IsBodyHtml = true;
                return mail;
            }
            catch (Exception ex)
            {
                LogManager.LogError("Error occured in client.GetMailInMailMessageFormat(Mail) in Mail.cs" + ex.Message + ".", ex);
                return null;
            }
        }

        static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}