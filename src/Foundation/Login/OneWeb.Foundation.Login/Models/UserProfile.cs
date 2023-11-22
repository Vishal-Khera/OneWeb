namespace OneWeb.Foundation.Login.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Role { get; set; }
        public bool IsApproved { get; set; }
        public string BusinessInformation { get; set; }
        public string language { get; set; }
    }
    public class GetUser
    {
        public UserProfile Result { get; set; }
        public bool Success { get; set; }
    }
}