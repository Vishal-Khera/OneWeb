namespace OneWeb.Foundation.Search.Models
{
    public class ResponseParameters
    {
        public string CurrentItemId { get; set; }
        public string SearchTerm { get; set; }
        public int TotalResultCount { get; set; }
        public int TotalPageCount { get; set; }
        public int CurrentPageCount { get; set; }
        public int CurrentResultCount { get; set; }
        public int IteratedResultCount { get; set; }
        public int IteratedPageCount { get; set; }
        public int RemainingResultCount { get; set; }
        public int RemainingPageCount { get; set; }
    }
}