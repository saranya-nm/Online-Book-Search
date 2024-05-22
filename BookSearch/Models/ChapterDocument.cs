namespace BookSearch.Models
{
    public class ChapterDocument
    {
        public List<Chapter> docs { get; set; }=new List<Chapter>();

        public int total { get; set; }

        public int limit { get; set; }
        public int offset { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
    }

    public class Chapter
    {
        public string _id { get; set; } = String.Empty;
        public string chapterName { get; set; }=String.Empty;
    }
}
