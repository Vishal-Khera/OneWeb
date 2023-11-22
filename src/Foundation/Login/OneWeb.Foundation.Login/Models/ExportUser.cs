using System.Collections.Generic;

namespace OneWeb.Foundation.Login.Models
{
    public class ExportUser
    {
        public string EmailAddress { get; set; }
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
        public string Date { get; set; }

        public int Id { get; set; }
    }
    public class ExportUserColumn
    {
        public string ColumnName { get; set; }
        public string Property { get; set; }
    }
    public class GetUsers
    {
        public List<ExportUser> UserList { get; set; }
        public bool Success { get; set; }
    }
}