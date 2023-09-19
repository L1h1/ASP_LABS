using ASP_LABS.Services.BookService;
using ASP_LABS.Services.GenreService;
using Microsoft.AspNetCore.Mvc;
using ASP_LABS.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing.Printing;

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



		[Route("Catalog/{genre}/{page=1}")]
        public async Task<ActionResult> Index(string genre,int page)
        {
            //список категорий
            var genreResponse = await _genreService.GetGenreListAsync();
            if (!genreResponse.Success)
            {
                return NotFound(genreResponse.ErrorMessage);
            }
            ViewBag.Genres = genreResponse.Data;
            //текущая категория
            ViewBag.CurrentGenre = genre == "all" ? "Выберите жанр" : genreResponse.Data.Items.First(x => x.NormalizedName == genre).Name; //костыль



            //список книг
            var bookResponse = await _bookService.GetBookListAsync(genre,page);
           if(!bookResponse.Success)
            {
                return NotFound(bookResponse.ErrorMessage);
            }

            if (Request.IsAjaxRequest())
            {
				return PartialView("_BooksPartial", bookResponse.Data);
			}
				
			return View(bookResponse.Data);
        }

        



    }


   
}
