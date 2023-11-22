using OneWeb.Feature.Login.Repositories;
using OneWeb.Foundation.Login.Models;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System.Web.Mvc;

namespace OneWeb.Feature.Login.Controllers
{
    public class UserController : StandardController
    {
        private IVirtualUserRepository _userRepository;
        private IUserSearchBox _userSearchBox;
        public UserController(IVirtualUserRepository userRepository, IUserSearchBox userSearchBox)
        {
            _userRepository = userRepository;
            _userSearchBox = userSearchBox;
        }
        public JsonResult Login(UserLogin login)
        {
            if (login != null && !string.IsNullOrEmpty(login.EmailAddress) && !string.IsNullOrEmpty(login.Password))
                return _userRepository.LoginUser(login);
            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Logout()
        {
            return _userRepository.Logout();
        }
        public JsonResult RegisterUser(UserProfile profile)
        {
            if (profile != null && !string.IsNullOrEmpty(profile.EmailAddress))
            {
                return _userRepository.RegisterUser(profile);
            }
            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult UpdateUserDetails(UserProfile profile)
        {
            if (profile != null && !string.IsNullOrEmpty(profile.EmailAddress))
            {
                return _userRepository.UpdateUserDetails(profile);
            }
            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult UpdateUserStatus(bool approve, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return _userRepository.UpdateUserStatus(approve, userId);
            }
            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public FileContentResult ExportUserData()
        {
            return File(_userRepository.ExportUserData(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx"); 
        }
        public JsonResult UpdatePassword(UserLogin login)
        {
            if (login != null &&  login.UserId!=0 && !string.IsNullOrEmpty(login.Password))
            {
                return _userRepository.UpdatePassword(login);
            }
            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ForgotPassword(UserProfile userProfile)
        {
            if (!string.IsNullOrEmpty(userProfile.EmailAddress))
            {
                return _userRepository.ForgotPassword(userProfile.EmailAddress, userProfile.language);
            }
            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult DeleteUser(string userId)
        {
            if(string.IsNullOrEmpty(userId))
                return new JsonResult()
                {
                    Data = string.Empty,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            return _userRepository.DeleteUser(userId);
        }
        public JsonResult SearchUsers(UserSearch userSearch)
        {
            if(userSearch != null)          
                return _userRepository.SearchUsers(userSearch);
            return new JsonResult()
            {
                Data = string.Empty,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateRole(int userId, string role)
        {
            
                return new JsonResult()
                {
                    Data = string.Empty,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            return _userRepository.UpdateRole(userId, role);
        }
        public JsonResult GetUserProfile()
        {
            return _userRepository.GetUserProfile();
        }
        public ActionResult RenderUserSearchBox()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item;
            if (datasourceItem != null)
            {
                return View("~/Views/Feature/Login/OneWebUserSearchBox.cshtml", _userSearchBox.RenderUserSearchBox(datasourceItem));
            }

            return new EmptyResult();
        }
        public ActionResult RenderLogin()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item;
            if (datasourceItem != null)
            {
                return View("~/Views/Feature/Login/OneWebUserLogin.cshtml", _userSearchBox.RenderUserLogin(datasourceItem));
            }

            return new EmptyResult();
        }

        public ActionResult RenderDownloadForm()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item;
            if (datasourceItem != null)
            {
                return View("~/Views/Feature/Login/OneWebDownloadForm.cshtml", _userSearchBox.RenderDownloadForm(datasourceItem));
            }

            return new EmptyResult();
        }

        
    }
}