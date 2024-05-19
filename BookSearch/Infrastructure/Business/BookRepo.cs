using BookSearch.Infrastructure.Interfaces;
using BookSearch.Models;

namespace BookSearch.Infrastructure.Business
{
    public class BookRepo : IBookRepo
    {
        private readonly IConfiguration _configuration;
        public const string BEARER_NAME = "Bearer";
        public BookRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Document> GetAllBooks()
        {
            return await GetBooks("Book");
        }

        public async Task<Document> SearchBooks(string searchValue, SearchType searchType)
        {
            string uri = "";
            switch (searchType)
            {
                case SearchType.match: uri = "=" + searchValue; break;
                case SearchType.negate: uri = "!=" + searchValue; break;
                case SearchType.regexmatch: uri = "=/" + searchValue + "/i"; break;
                case SearchType.regexnegate: uri = "!=/" + searchValue + "/i"; break;
                default: break;
            }
            return await GetBooks(uri);
        }

        public async Task<List<Chapter>> GetAllChapters(string url)
        {
            using HttpClient client = new HttpClient();
            {
                client.BaseAddress = new Uri(_configuration["BookAPI"]);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(BEARER_NAME,
                    Environment.GetEnvironmentVariable("BEARER_KEY"));
                var responseTask = client.GetAsync(url);
                responseTask.Wait();
                ChapterDocument chapter = new ChapterDocument();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<ChapterDocument>();
                    readTask.Wait();

                    chapter = readTask.Result;
                }
                if (chapter != null)
                    return chapter.docs;
                else
                    return new List<Chapter>();
            }
        }

        private async Task<Document> GetBooks(string url)
        {
            Document books = new Document();
            using HttpClient client = new HttpClient();
            {
                client.BaseAddress = new Uri(_configuration["BookAPI"]);
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadFromJsonAsync<Document>();
                    readtask.Wait();

                    books = readtask.Result;

                }
            }
            return books;
        }


    }
}
