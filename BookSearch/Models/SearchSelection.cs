using System.ComponentModel.DataAnnotations;


namespace BookSearch.Models
{
    public enum SearchType { Match, Negate, Regexmatch, Regexnegate, AllBooks }

    public enum SortDirection { Ascending,Descending,Default}
    public class SearchSelection
    {
        [Display (Name= "Search")]
        [Required (ErrorMessage="Please Enter Search Value.")]
        public string SearchValue { get; set; }
        
        public SearchType SearchType { get; set; }
    }
}
