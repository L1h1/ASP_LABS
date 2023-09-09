using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASP_LABS.Domain.Entities;
using ASP_LABS.Services.BookService;
using ASP_LABS.Services.GenreService;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace ASP_LABS.Areas.Admin
{
	[Authorize]

	public class CreateModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private List<Genre> _genreList;

        public CreateModel(IBookService bookService,IGenreService genreService)
        {

            _bookService = bookService;
            _genreService = genreService;
            _genreList = _genreService.GetGenreListAsync().Result.Data.Items;
            Genres = new SelectList(_genreList,"Id","Name");
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public IFormFile? Image { get; set; }
        [BindProperty]
        public Book Book { get; set; } = default!;

        [BindProperty]
        public int GenreId { get; set; }

        public SelectList Genres { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Book.Genre = _genreList.Find(g=>g.Id== GenreId);
            Book.GenreId= GenreId;
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }

            var response = await _bookService.CreateBookAsync(Book,Image);
            return RedirectToPage("./Index");
        }
    }
}
