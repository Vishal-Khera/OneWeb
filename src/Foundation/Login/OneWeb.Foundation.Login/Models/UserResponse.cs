using System.Collections.Generic;

namespace OneWeb.Foundation.Login.Models
{
    public class UserResponse
    {
        public string EmailAddress { get; set; }

        public int UserId { get; set; }
        public string Status { get; set; }
        public string RedirectURL { get; set; }
    }
    public class ApiResponse
    {
        public string Token { get; set; }

        public string EmailAddress { get; set; }
      
        public string UserId { get; set; }
        public bool Success { get; set; }
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public List<ExportUser> UserList { get; set; }
        public UserProfile Result { get; set; }
    }
    public class ApprovedUserResponse
    {
         
        public string UserId { get; set; }
        public bool Success { get; set; }
        public bool isApprovedUser { get; set; }
    }
    public struct StatusCode
    {
        public static string NotFound = "Not Found";
        public static string AlreadyExists = "Already Exists";
        public static string Success = "Success";
        public static string Error = "Error";
        public static string WrongCredentials = "Wrong Credentials";
        public static string Approved = "Approved";
        public static string NotApproved = "NotApproved";
    }
}