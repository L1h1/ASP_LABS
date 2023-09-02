using ASP_LABS.Services.BookService;
using ASP_LABS.Services.GenreService;
using Microsoft.AspNetCore.Mvc;

namespace ASP_LABS.Controllers
{
    public class BookController : Controller
    {
        IBookService _bookService;
        IGenreService _genreService;

        public BookController(IBookService bookService, IGenreService genreService)
        {
            _bookService = bookService;
            _genreService = genreService;
        }

        [Route("{controller=Book}/{action=Index}/{genre}/{page=1}")]
        public async Task<ActionResult> Index(string genre,int page)
        {
            var bookResponse = await _bookService.GetBookListAsync(genre,page);
            //список категорий
            var genreResponse = await _genreService.GetGenreListAsync();
            ViewBag.Genres = genreResponse.Data;
            //текущая категория
            ViewBag.CurrentGenre = genre == "all" ? "Выберите жанр" : genreResponse.Data.Items.First(x=>x.NormalizedName==genre).Name; //костыль

            if (!bookResponse.Success)
                return NotFound(bookResponse.ErrorMessage);
			return View(bookResponse.Data);
        }
    }
}
