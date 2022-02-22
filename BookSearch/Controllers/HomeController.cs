using BookSearch.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace BookSearch.Controllers
{
    public class HomeController : Controller
    {
        public Document books=null;
        public const string _baseUri = "https://the-one-api.dev/v2/";
        public const string BEARER_NAME= "Bearer";
        

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Home View
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ViewBag.ErrorMessage = "";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        #region "Books"

        /// <summary>
        /// This method returns all the Books from the API.
        /// Its a call from the Show All Books from the Home page
        /// </summary>
        /// <returns></returns>
        //public IActionResult AllBooks()
        //{
        //    try
        //    {
                
        //            using HttpClient client = new HttpClient();
        //            {
        //                client.BaseAddress = new Uri(_baseUri);
        //                var responseTask = client.GetAsync("Book");
        //                responseTask.Wait();
        //                Document docs = null;


        //                //JsonConvert.DeserializeObject<object>(jsonString)
        //                var result = responseTask.Result;
        //                if (result.IsSuccessStatusCode)
        //                {
        //                    var readtask = result.Content.ReadFromJsonAsync<Document>();
        //                    readtask.Wait();

        //                    docs = readtask.Result;

        //                }
        //                books = new Document();
        //                books = docs;
        //                ViewData["books"] = docs.docs.Count > 0 ? docs.docs : null;
        //                return View();
        //            }
               
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }

        //}
        /// <summary>
        /// This is a filter call to Filter and Seach books
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public IActionResult SearchBooks(string searchValue="", SearchType searchType=SearchType.AllBooks)
        {
            try
            {

        if (ModelState.IsValid || searchType==SearchType.AllBooks)
        {
            string uri = "";
                switch (searchType)
                {
                    case SearchType.Match: uri = "?name=" + searchValue; break;
                    case SearchType.Negate: uri = "?name!=" + searchValue; break;
                    case SearchType.Regexmatch: uri = "?name=/" + searchValue + "/i"; break;
                    case SearchType.Regexnegate: uri = "?name!=/" + searchValue + "/i"; break;
                    default: break;
                }

                using HttpClient client = new HttpClient();
                {

                    client.BaseAddress = new Uri(_baseUri);
                    var responseTask = client.GetAsync("book" + uri);
                    responseTask.Wait();
                    Document docs = new Document();


                    //JsonConvert.DeserializeObject<object>(jsonString)
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readtask = result.Content.ReadFromJsonAsync<Document>();
                        readtask.Wait();

                        docs = readtask.Result;

                    }

                    ViewData["books"] = docs.docs.Count > 0 ? docs.docs : null;
                        
                    return View("AllBooks");
                }
                }
                else
                {
                    //ViewBag.ErrorMessage = "not empty";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        #endregion
        #region "Chapters"

      /// <summary>
      /// Retrieves all the book chapters
      /// </summary>
      /// <param name="url"></param>
      /// <returns></returns>
        public void GetAllChapters(string url)
        {
            using HttpClient client = new HttpClient();
            {
                client.BaseAddress = new Uri(_baseUri);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(BEARER_NAME,
                    Environment.GetEnvironmentVariable("BEARER_KEY"));
                var responseTask = client.GetAsync(url);
                responseTask.Wait();
                ChapterDocument docs = new ChapterDocument();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadFromJsonAsync<ChapterDocument>();
                    readtask.Wait();

                    docs = readtask.Result;

                }
                ViewData["chapter"] = docs.docs;
            }
        }
      
        /// <summary>
        /// This method builds a url to sort/page the chapters
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <param name="SortOrder"></param>
        /// <returns></returns>
        public IActionResult UrlBuilder(string Name, string Id, int Page=1,string Limit="None",
            SortDirection SortOrder = SortDirection.Default,string searchValue="")
        
        {
            string url;
            if (searchValue == "")
                url = $"book/{Id}/chapter";
            else
                url = $"chapter?chapterName=/{searchValue}/i";

            if (SortOrder != SortDirection.Default)
                if (searchValue == "")
                    url += "?";
                else
                    url+="&";
                    switch (SortOrder)
                    {
                        case SortDirection.Ascending: url += "sort=chapterName:asc"; break;
                        case SortDirection.Descending: url += "sort=chapterName:desc"; break;
                    }
            if (Limit != "None")
            {
                if (SortOrder != SortDirection.Default)
                    url += "&";
                else
                    url += "?";
                
                url += $"limit=10&page={Page}";
            }
            ViewBag.searchValue=searchValue;
            ViewBag.bookName = Name;
            ViewBag.id = Id;
            ViewBag.page = Page;
            ViewBag.limit = Limit;
            ViewBag.sortOrder = SortOrder;
            GetAllChapters(url);
            return View("Chapters");
        }

      


        #endregion
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}