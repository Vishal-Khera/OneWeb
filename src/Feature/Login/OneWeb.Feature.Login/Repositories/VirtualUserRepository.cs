using OneWeb.Foundation.Login.Models;
using OneWeb.Foundation.Login.Repositories;
using Sitecore.Security.Accounts;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System.Web.Mvc;
using Sitecore.Security.Authentication;
using Sitecore.Globalization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Items;
using OneWeb.Foundation.Serialization;
using System.Web;
using System;

namespace OneWeb.Feature.Login.Repositories
{
    public class VirtualUserRepository : IVirtualUserRepository 
    {
        private IUserService _userService;
        public VirtualUserRepository(IUserService userService)
        {
            _userService = userService;
        }
        public JsonResult LoginUser(UserLogin login)
        {
            ApiResponse response = _userService.VerifyUser(login);
            response.EmailAddress = login.EmailAddress;
            if (response == null || !response.Success || !CreateVirtualUser(response))
            {
                return new JsonResult()
                {
                    Data = new UserResponse()
                    {
                        EmailAddress = login.EmailAddress,
                        Status = Translate.Text(_userService.LoginDictionary(response.ResponseCode)),
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = new UserResponse()
                {
                    EmailAddress = login.EmailAddress,
                    Status = Translate.Text(_userService.LoginDictionary(response.ResponseCode)),
                    RedirectURL = "/"
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public IRenderingModelBase GetModel()
        {
            throw new System.NotImplementedException();
        }
        public JsonResult RegisterUser(UserProfile profile)
        {
            return new JsonResult()
            {
                Data = _userService.RegisterUser(profile),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateUserDetails(UserProfile profile)
        {
           return new JsonResult()
            {
                Data = _userService.UpdateUserDetails(profile),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateUserStatus(bool approve, string userId)
        {
            return new JsonResult()
            {
                Data = _userService.UpdateUserStatus(approve, userId),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public byte[] ExportUserData()
        {
            return _userService.ExportUserData();
        }
        public JsonResult UpdatePassword(UserLogin loginDetails)
        {
            return new JsonResult()
            {
                Data = _userService.UpdatePassword(loginDetails),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult ForgotPassword(string email,string language)
        {
            return new JsonResult()
            {
                Data = _userService.ForgotPassword(email,language),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult Logout()
        {
            var activeUser = AuthenticationManager.GetActiveUser();
            if (_userService.Logout(activeUser.Name))
            {
                activeUser.RuntimeSettings.AddedRoles.Clear();
                activeUser.Roles.RemoveAll();
                activeUser.RuntimeSettings.Save();
                AuthenticationManager.Logout();
                return new JsonResult()
                {
                    Data = new UserResponse()
                    {
                        Status = StatusCode.Success
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = new UserResponse()
                {
                    Status = StatusCode.Error
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public bool CreateVirtualUser(ApiResponse response)
        {
            string defaultRole = GetDefaultRole();
            User virtualUser = AuthenticationManager.BuildVirtualUser(response.EmailAddress, true);
            virtualUser.RuntimeSettings.AddedRoles.Clear();
            if(Role.Exists(defaultRole))
                virtualUser.RuntimeSettings.AddedRoles.Add(defaultRole);
            virtualUser.RuntimeSettings.Save();
            virtualUser.Profile.Save();
                HttpCookie cookie1 = new HttpCookie("UserId")
                {
                    Expires = DateTime.Now.AddHours(3),
                    Path = "/",
                    Value = response.UserId
                };
                HttpContext.Current.Response.Cookies.Add(cookie1);
            HttpCookie cookie2 = new HttpCookie("SC_Name")
            {
                Expires = DateTime.Now.AddHours(3),
                Path = "/",
                Value = response.EmailAddress
            };
            HttpContext.Current.Response.Cookies.Add(cookie2);
            HttpCookie cookie = new HttpCookie("SC_USR")
                {
                    Expires = DateTime.Now.AddHours(3),
                    Path = "/",
                    Value = response.Token
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
        }
        public string GetDefaultRole()
        {
            Item settings = SiteExtensions.GetSettingsItem();
            return settings.GetFieldValue(SiteConfigurations.Fields.DefaultRole_FieldName);
        }
        public JsonResult DeleteUser(string userId)
        {
            return new JsonResult()
            {
                Data = _userService.DeleteUser(userId),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult SearchUsers(UserSearch userSearch)
        {
            return new JsonResult()
            {
                Data = _userService.SearchUsers(userSearch),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateRole(int userId, string role)
        {
            return new JsonResult()
            {
                Data = _userService.UpdateRole(userId, role),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetUserProfile()
        {           
            if (HttpContext.Current.Request.Cookies["SC_Name"] != null)
            {
                string email = HttpContext.Current.Request.Cookies["SC_Name"].Value;
                return new JsonResult()
                {
                    Data = _userService.GetUserProfile(email),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            { 
                Data = StatusCode.Error,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
       
    }
}