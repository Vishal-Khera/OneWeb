using OneWeb.Foundation.Login.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;

namespace OneWeb.Foundation.Login.Repositories
{
    public interface IUserService : IModelRepository
    {
        ApiResponse VerifyUser(UserLogin login);
        UserResponse RegisterUser(UserProfile profile);
        UserResponse UpdateUserDetails(UserProfile profile);
        UserResponse UpdateUserStatus(bool approve, string userId);
        byte[] ExportUserData();
        UserResponse UpdatePassword(UserLogin login);
        UserResponse ForgotPassword(string email,string language);
        UserResponse DeleteUser(string email);
        Users SearchUsers(UserSearch userSearch);
        UserResponse UpdateRole(int userId, string role);
        UserProfile GetUserProfile(string name);
        bool IsLoggedInUser();
        string LoginDictionary(int key);
        bool Logout(string userId);
    }
}