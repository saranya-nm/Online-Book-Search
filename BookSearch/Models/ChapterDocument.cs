namespace BookSearch.Models
{
    public class ChapterDocument
    {
        public List<Chapter> docs { get; set; }

        public int total { get; set; }

        public int limit { get; set; }
        public int offset { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
    }
}
