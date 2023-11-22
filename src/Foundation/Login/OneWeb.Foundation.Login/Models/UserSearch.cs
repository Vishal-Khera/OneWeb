using System.Collections.Generic;

namespace OneWeb.Foundation.Login.Models
{
    public class UserSearch
    {
        public string Keyword { get; set; }
        public string PageSize { get; set; }
        public string PagesToSkip { get; set; }
    }
    public class Users
    {
        public List<ExportUser> UsersInfo { get; set; }
        public int TotalPageCount { get; set; }
        public int FirstPage { get; set; }
        public int RemainingPageCount { get; set; }
        public int CurrentPage { get; set; }
        public List<int> Pages { get; set; }
        public int BackPage { get; set; }
        public int NextPage { get; set; }
        public int StartData { get; set; }
        public int EndData { get; set; }
        public int TotalResult { get; set; }

        public string dot { get; set; }
    }
}