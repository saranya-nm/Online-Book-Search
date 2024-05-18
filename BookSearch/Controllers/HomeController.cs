using BookSearch.Infrastructure.Interfaces;
using BookSearch.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace BookSearch.Controllers
{
    public class HomeController : Controller
    {
        public IBookRepo _bookRepo;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,IBookRepo bookRepo)
        {
            _logger = logger;
            _bookRepo = bookRepo;
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
        public async Task<IActionResult> AllBooks()
        {
            try
            {    
                var docs= await _bookRepo.GetAllBooks();
                ViewData["books"] = docs.docs.Count > 0 ? docs.docs : null;
                return View();
        
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// This is a filter call to Filter and Seach books
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SearchBooks(string searchValue, SearchType searchType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var docs = await _bookRepo.SearchBooks(searchValue, searchType);
                    ViewData["books"] = docs.docs.Count > 0 ? docs.docs : null;
                        
                    return View("AllBooks");   
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
       
      
        /// <summary>
        /// This method builds a url to sort/page the chapters
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <param name="SortOrder"></param>
        /// <returns></returns>
        public async Task<IActionResult> UrlBuilder(string Name, string Id, int Page=1,string Limit="None",
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
            var chap=await _bookRepo.GetAllChapters(url);
            ViewData["books"] = chap.Count > 0 ? chap : null;
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