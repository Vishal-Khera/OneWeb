namespace OneWeb.Foundation.Login.Models
{
    public class UserRole
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public string SecretKey { get; set; }
    }
}