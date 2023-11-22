using System.Web.Mvc;
using OneWeb.Foundation.Login.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;

namespace OneWeb.Feature.Login.Repositories
{
    public interface IVirtualUserRepository : IModelRepository
    {
        JsonResult LoginUser(UserLogin login);
        JsonResult RegisterUser(UserProfile profile);
        JsonResult UpdateUserDetails(UserProfile profile);
        JsonResult UpdateUserStatus(bool approve, string userId);
        byte[] ExportUserData();
        JsonResult UpdatePassword(UserLogin login);
        JsonResult ForgotPassword(string email, string language);
        JsonResult Logout();
        JsonResult DeleteUser(string userId);
        JsonResult SearchUsers(UserSearch userSearch);
        JsonResult UpdateRole(int userId, string role);
        JsonResult GetUserProfile();
    }
}