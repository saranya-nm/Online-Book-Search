using System.ComponentModel.DataAnnotations;


namespace BookSearch.Models
{
    public enum SearchType { match, negate, regexmatch, regexnegate }

    public enum SortDirection { Ascending,Descending,Default}
    public class SearchSelection
    {

        [Required (ErrorMessage="Please Enter Search Value.")]
        public string SearchValue { get; set; }
        
        public SearchType SearchType { get; set; }
    }
}
