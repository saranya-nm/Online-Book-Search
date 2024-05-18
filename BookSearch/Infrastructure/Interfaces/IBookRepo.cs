using BookSearch.Models;

namespace BookSearch.Infrastructure.Interfaces
{
    public interface IBookRepo
    {
        Task<Document> GetAllBooks();
        Task<Document> SearchBooks(string searchValue, SearchType searchType);
        Task<List<Chapter>> GetAllChapters(string url);
    }
}