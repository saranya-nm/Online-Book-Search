namespace BookSearch.Models
{
    public class Document
    {
        public List<Book> docs { get; set; } = new List<Book>();

        public int Total { get; set; }

        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
    }
    public class Book
    {
        public string _id { get; set; }=String.Empty;
        public string Name { get; set; }=String.Empty ;
    }
}
