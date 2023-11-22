using ClosedXML.Excel;
using OneWeb.Foundation.Login.Models;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using Newtonsoft.Json;
using System.Web;
using Settings = Sitecore.Configuration.Settings;

namespace OneWeb.Foundation.Login.Repositories
{
    public class UserService : IUserService
    {
        private IApiService _apiService;
        private const string _apiKeyValue = "UserManagerKey";
        public UserService()
        {

        }
        public UserService(IApiService apiService)
        {
            _apiService = apiService;
        }
        public UserResponse RegisterUser(UserProfile profile)
        {
             
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.RegisterUser_FieldName);
            string parameters = JsonConvert.SerializeObject(profile);
            var response = _apiService.Post<ApiResponse>(url, parameters);
            if (response != null && response.Success)
            {
                if (SendRegistrationMail())
                {
                    using (new LanguageSwitcher(profile.language = SiteExtensions.GetLanguage(profile.language, Context.Language.Name)))
                    {
                        MailTemplate mailData = GetMailTemplateData(SiteConfigurations.Fields.RegistrationMail_FieldName, profile.language);
                        if (mailData != null)
                        {

                            mailData.To = new List<string>
                        {
                            profile.EmailAddress
                        };
                            mailData.Body = mailData.Body.Replace("_Email", profile.EmailAddress);
                            LinkModel verificationPage = LinkModelFromSettings(SiteConfigurations.Fields.RegistrationVerificationPage_FieldName);
                            mailData.Body = string.Format(mailData.Body, FormatLink(verificationPage, response.UserId));
                            EmailExtensions.SendEmail(mailData.From, mailData.To.FirstOrDefault(), mailData.CC?.FirstOrDefault(), mailData.Bcc?.FirstOrDefault(), mailData.Subject, mailData.Body, "");
                        }
                    }
                }
                LinkModel link = LinkModelFromSettings(SiteConfigurations.Fields.ThankyouPageForRegistration_FieldName);
                return new UserResponse()
                {
                    EmailAddress = profile.EmailAddress,
                    Status = Translate.Text(LoginDictionary(response.ResponseCode)),
                    RedirectURL = link.Url
                };
            }
            return new UserResponse()
            {
                EmailAddress = profile.EmailAddress,
                Status = Translate.Text(LoginDictionary(response.ResponseCode)),
            };
        }
        private string FormatLink(LinkModel pageLink, string userId)
        {
            if (pageLink != null && !string.IsNullOrEmpty(pageLink.Url) && !string.IsNullOrEmpty(pageLink.Text))
            {
                StringBuilder link = new StringBuilder();
                link.Append("<a href=");
                link.Append("https://" + Context.Site.TargetHostName + pageLink.Url + "?id=" + userId + ">");
                link.Append(pageLink.Text);
                link.Append("</a>");
                return link.ToString();
            }
            return null;
        }
        public bool IsLoggedInUser()
        {
            User user = AuthenticationManager.GetActiveUser();
            return !user.Name.Equals("extranet\\Anonymous", StringComparison.OrdinalIgnoreCase);
        }
        public bool IsLoggedInCookieUser()
        {
            if (HttpContext.Current.Request.Cookies["SC_USR"] != null)
            {
                string cookie = HttpContext.Current.Request.Cookies["SC_USR"].Value;
                if (cookie != "")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public LinkModel LinkModelFromSettings(string fieldName)
        {
            Item settings = SiteExtensions.GetSettingsItem();
            return new LinkModel(settings, fieldName);
        }
        public MailTemplate GetMailTemplateData(string fieldName, string  language)
        {
            MailTemplate mailTemplate = null;
            Item settings = SiteExtensions.GetSettingsItem();
            //var mail = settings[fieldName];
            using (new LanguageSwitcher(language = SiteExtensions.GetLanguage(language, Context.Language.Name)))
            {
                var mail = settings.GetReferencedItem(fieldName);

                if (mail != null)
                {
                    string CC = mail.GetFieldValue(OneWebUserEmailTemplate.Fields.CC_FieldName);
                    string Bcc = mail.GetFieldValue(OneWebUserEmailTemplate.Fields.Bcc_FieldName);
                    mailTemplate = new MailTemplate
                    {
                        From = mail.GetFieldValue(OneWebUserEmailTemplate.Fields.From_FieldName),
                        CC = !string.IsNullOrEmpty(CC) ? new List<string>(CC.Split(',')) : new List<string>(),
                        Bcc = !string.IsNullOrEmpty(Bcc) ? new List<string>(Bcc.Split(',')) : new List<string>(),
                        Subject = mail.GetFieldValue(OneWebUserEmailTemplate.Fields.Subject_FieldName),
                        Body = mail.GetFieldValue(OneWebUserEmailTemplate.Fields.Body_FieldName)
                    };
                }
            }
            return mailTemplate;
        }
        public IRenderingModelBase GetModel()
        {
            throw new System.NotImplementedException();
        }
        public bool SendRegistrationMail()
        {
            Item settings = SiteExtensions.GetSettingsItem();
            return settings.GetFieldValue(SiteConfigurations.Fields.SendRegistrationMail_FieldName) == "1";
        }
        public ApiResponse VerifyUser(UserLogin login)
        {
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.UserLogin_FieldName);
            string parameters = JsonConvert.SerializeObject(login);
            return _apiService.Post<ApiResponse>(url, parameters);
        }
        public UserResponse UpdateUserDetails(UserProfile profile)
        {
            string token = HttpContext.Current.Request.Cookies["SC_USR"]?.Value;
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.UpdateUserProfile_FieldName);
            string parameters = JsonConvert.SerializeObject(profile);
            var response = _apiService.Patch<ApiResponse>(url, parameters, token);
            if (response.Success)
            {
                LinkModel link = LinkModelFromSettings(SiteConfigurations.Fields.ThankyouPageForUpdateProfile_FieldName);
                return new UserResponse()
                {
                    EmailAddress = profile.EmailAddress,
                    Status = StatusCode.Success,
                    RedirectURL = link.Url
                };
            }
            return new UserResponse()
            {
                EmailAddress = profile.EmailAddress,
                Status = StatusCode.Error,
            };
        }
        public UserResponse UpdateUserStatus(bool approve, string userId)
        {
            string apiKey = GetAPIKey();
            if (apiKey == null)
                return default;
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.UpdateUserStatus_FieldName);
            UserStatus status = new UserStatus() { Approve = approve, UserId = userId, ApiKey = apiKey }; 
            string parameters = JsonConvert.SerializeObject(status);
            string requestUri = url + "?Approve=" + approve + "&userId=" + userId + "&SecretKey=" + apiKey;
            var response = _apiService.Patch<ApiResponse>(requestUri, parameters);
            if (response.Success)
            {
                return new UserResponse()
                {
                    EmailAddress = userId,
                    Status = StatusCode.Success
                };
            }
            return new UserResponse()
            {
                EmailAddress = userId,
                Status = StatusCode.Error
            };
        }
        public byte[] ExportUserData()
        {
            string apiKey = GetAPIKey();
            if (apiKey == null)
                return default;

            string url = GetAPIUrl(OneWebUserApiSettings.Fields.GetUsers_FieldName);
            var requestUri = new Uri($"?SecretKey={HttpUtility.UrlEncode(apiKey)}", UriKind.Relative);
            var response = _apiService.Get<ApiResponse>(url,requestUri, apiKey);
            if (response.Success)
            {
                List<ExportUser> users = response.UserList;
                if (users.Any())
                {
                    Item settings = SiteExtensions.GetSettingsItem();
                    List<Item> columnsData = settings.GetMultiListItems(SiteConfigurations.Fields.ExportUserColumns_FieldName).ToList();
                    if (columnsData.Any())
                    {
                        List<ExportUserColumn> columns = new List<ExportUserColumn>();
                        columnsData.ForEach(x => columns.Add(new ExportUserColumn()
                        {
                            ColumnName = x.GetFieldValue(OneWebExportUserColumn.Fields.Name),
                            Property = x.GetFieldValue(OneWebExportUserColumn.Fields.Property)
                        }));
                        using (var workbook = new XLWorkbook())
                        {
                            IXLWorksheet worksheet = workbook.Worksheets.Add("Users");
                            int index = 1;
                            foreach (var column in columns)
                            {
                                var property = users.FirstOrDefault().GetType().GetProperty(column.Property);
                                if (property != null)
                                {
                                    worksheet.Cell(1, index).Value = column.ColumnName;
                                    index++;
                                }
                            }
                            int usercount = 1;
                            foreach (var user in users)
                            {
                                foreach (var column in columns)
                                {
                                    int columnIndex = 0;
                                    var property = users.FirstOrDefault().GetType().GetProperty(column.Property);
                                    if (property != null)
                                    {
                                        worksheet.Cell(usercount + 1, columns.IndexOf(column) + 1).Value = users[columnIndex].GetType().GetProperty(column.Property).GetValue(users[usercount - 1]);
                                        columnIndex++;
                                    }
                                }
                                usercount++;
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return content;
                            }
                        }
                    }
                }
            }
            return null;
        }
        public UserResponse UpdatePassword(UserLogin login)
        {
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.UpdatePassword_FieldName);
            string parameters = JsonConvert.SerializeObject(login);
            var response = _apiService.Patch<ApiResponse>(url, parameters);
            if (response.Success)
            {
                LinkModel link = LinkModelFromSettings(SiteConfigurations.Fields.ThankyouPageForUpdatePassword_FieldName);
                return new UserResponse()
                {
                    UserId = login.UserId,
                    Status = StatusCode.Success,
                    RedirectURL=link.Url
                };
            }
            return new UserResponse()
            {
                UserId = login.UserId,
                Status = StatusCode.Error
            };
        }
        public UserResponse ForgotPassword(string email,string language)
        {
            string apiKey = GetAPIKey();
            if (apiKey == null)
                return default;
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.IsApprovedUser_FieldName);
            var requestUri = new Uri($"?SecretKey={HttpUtility.UrlEncode(apiKey)}&EmailAddress="+email, UriKind.Relative);
            var response = _apiService.Get<ApprovedUserResponse>(url, requestUri, apiKey);        
            if (response.Success && response.isApprovedUser)
            {
                MailTemplate mailData = GetMailTemplateData(SiteConfigurations.Fields.ForgotPasswordMail_FieldName,language);
                if (mailData != null)
                {
                    using (new LanguageSwitcher(language = SiteExtensions.GetLanguage(language, Context.Language.Name)))
                    {
                        LinkModel pageLink = LinkModelFromSettings(SiteConfigurations.Fields.UpdatePasswordPage_FieldName);
                        mailData.Body = mailData.Body.Replace("_Email", email);
                        mailData.Body = string.Format(mailData.Body, FormatLink(pageLink, response.UserId));
                        mailData.To = new List<string>
                    {
                        email
                    };
                        EmailExtensions.SendEmail(mailData.From, mailData.To[0], mailData.CC[0], mailData.Bcc[0], mailData.Subject, mailData.Body, "");
                    }
                }
                LinkModel link = LinkModelFromSettings(SiteConfigurations.Fields.ThankyouPageForForgotPassword_FieldName);
                return new UserResponse()
                {
                    EmailAddress = email,
                    Status = StatusCode.Success,
                    RedirectURL=link.Url
                    
                };
            }
            return new UserResponse()
            {
                EmailAddress = email,
                Status = Translate.Text(LoginDictionary(1005))
            };
        }
        public UserResponse DeleteUser(string userId)
        {
            string apiKey = GetAPIKey();
            if (apiKey == null)
                return default;
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.DeleteUser_FieldName);
            string requestUri = url + "?UserId=" + userId + "&SecretKey=" + apiKey;
            DeleteUser parameters = new DeleteUser() { UserId = userId, SecretKey =apiKey};
            string parameters1 = JsonConvert.SerializeObject(parameters);
            var response = _apiService.Patch<ApiResponse>(requestUri, parameters1);
            if (response.Success)
            {
                return new UserResponse()
                {
                    EmailAddress = userId,
                    Status = StatusCode.Success
                };
            }
            return new UserResponse()
            {
                EmailAddress = userId,
                Status = StatusCode.Error
            };
        }
        public Users SearchUsers(UserSearch userSearch)
        {
            string apiKey = GetAPIKey();
            if (apiKey == null)
                return default;
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.GetUsers_FieldName);
            var requestUri = new Uri($"?SecretKey={HttpUtility.UrlEncode(apiKey)}", UriKind.Relative);
            var response = _apiService.Get<ApiResponse>(url, requestUri, apiKey);           
            if (response.Success && response.UserList.Any())
            {
                var usersInfo = response.UserList;
                Users users = new Users();
                string keyword = userSearch.Keyword;
                int pageSize = int.TryParse(userSearch.PageSize, out pageSize) ? pageSize : 10;
                int pagesToSkip = int.TryParse(userSearch.PagesToSkip, out pagesToSkip) ? pagesToSkip : 0;
                if (!string.IsNullOrEmpty(keyword))
                    usersInfo = usersInfo.Where(i => i.EmailAddress.ToLower().Contains(userSearch.Keyword.ToLower()) || i.FirstName.ToLower().Contains(userSearch.Keyword.ToLower()) || i.LastName.ToLower().Contains(userSearch.Keyword.ToLower())).ToList();
                users.UsersInfo = usersInfo.Skip((pagesToSkip) * pageSize).Take(pageSize).ToList();
                double totalPages = ((double)usersInfo.Count()) / ((double)pageSize);
                int totalPagesI = (int)Math.Ceiling(totalPages);
                users.TotalPageCount = totalPagesI == 0 ? 1 : totalPagesI;
                users.FirstPage = 1;
                users.RemainingPageCount = users.TotalPageCount - pagesToSkip;
                users.CurrentPage = pagesToSkip + 1;
                users.BackPage = pagesToSkip == 0 ? 0 : pagesToSkip;
                users.NextPage = pagesToSkip == totalPagesI - 1 ? totalPagesI : pagesToSkip + 2;
                users.StartData = (pagesToSkip * pageSize) + 1;
                if(users.UsersInfo.Count() < pageSize)
                {                   
                    if(users.UsersInfo.Count == 1)
                    users.EndData = users.StartData;
                    else
                        users.EndData = users.StartData+(users.UsersInfo.Count- 1);
                }
                else
                {
                    users.EndData = (pagesToSkip * pageSize) + pageSize;
                }
                
                
                users.TotalResult = usersInfo.Count();
                if (users.TotalPageCount != 1)
                {
                    if (pagesToSkip == totalPages)
                    {
                        users.dot = "";
                        users.Pages = new List<int>() { totalPagesI - 3, totalPagesI - 2, totalPagesI - 1 };
                    }
                    else
                    {
                        if(pagesToSkip + 1==Math.Ceiling(totalPages))
                        {
                            users.dot = "";
                        }
                       else if(pagesToSkip+2== Math.Ceiling(totalPages))
                        {
                            users.dot = "";
                        }
                        else if (pagesToSkip + 3 == Math.Ceiling(totalPages))
                        {
                            users.dot = "";
                        }
                        else
                        {
                            users.dot = "...";
                        }

                        users.Pages = new List<int>() { pagesToSkip, pagesToSkip + 1, pagesToSkip + 2 };
                    }
                    users.Pages = users.Pages.Where(i => i < totalPages && i > 0).ToList();
                }
                return users;
            }
            return null;
        }
        public UserResponse UpdateRole(int userId, string role)
        {
            string apiKey = GetAPIKey();
            if (apiKey == null)
                return default;
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.UpdateUserRole_FieldName);
            UserRole status = new UserRole() {  Role = role, UserId = userId, SecretKey = apiKey };
            string parameters = JsonConvert.SerializeObject(status);
            var response = _apiService.Patch<ApiResponse>(url, parameters);
            if (response.Success)
            {
                return new UserResponse()
                {
                    UserId = userId,
                    Status = StatusCode.Success
                };
            }
            return new UserResponse()
            {
                UserId = userId,
                Status = StatusCode.Error
            };
        }
        public UserProfile GetUserProfile(string email)
        {
            string token = HttpContext.Current.Request.Cookies["SC_USR"]?.Value;
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.GetUserProfile_FieldName);
            var requestUri = new Uri($"?EmailAddress={HttpUtility.UrlEncode(email)}", UriKind.Relative);
            var response = _apiService.Get<ApiResponse>(url, requestUri, email,token);
            if (response.Success)
            {
                return response.Result;
            }
            return null;
        }
        public string GetAPIUrl(string fieldName)
        {
            Item settings = SiteExtensions.GetSettingsItem();
            Item apiSettings = settings.GetReferencedItem(SiteConfigurations.Fields.ApiSettings_FieldName);
            if (apiSettings != null)
            {
                string startPath = apiSettings.GetFieldValue(OneWebUserApiSettings.Fields.BaseUrl_FieldName);
                string relativePath = apiSettings.GetFieldValue(fieldName);
                return startPath + relativePath;
            }
            return string.Empty;
        }

        public string GetAPIKey()
        {             
            string apikey = Settings.GetSetting(_apiKeyValue);
            return apikey;
        }
        public string LoginDictionary(int key)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            dictionary.Add(1000, "Success");
            dictionary.Add(1001, "Error");
            dictionary.Add(1002, "OneWeb-Login-No-User-Found");
            dictionary.Add(1003, "OneWeb-Login-User-Already-Exists");
            dictionary.Add(1004, "OneWeb-Login-Wrong-Credentials");
            dictionary.Add(1005, "OneWeb-Login-User-Not-Approved");
            dictionary.TryGetValue(key, out string value);
            return value;
        }
        public bool Logout(string userId)
        {
            string token = HttpContext.Current.Request.Cookies["SC_USR"]?.Value;
            string url = GetAPIUrl(OneWebUserApiSettings.Fields.Logout_FieldName);
            string uri = url + "?EmailAddress=" + userId;
            var response = _apiService.Post<ApiResponse>(uri, userId, token);
            HttpCookie currentUserCookie = HttpContext.Current.Request.Cookies["UserId"];
            HttpContext.Current.Response.Cookies.Remove("UserId");
            HttpContext.Current.Response.Cookies.Remove("SC_USR");
            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            currentUserCookie.Value = null;
            HttpContext.Current.Response.SetCookie(currentUserCookie);
            return response.Success;
        }
    }
}