using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP_LABS.Domain.Entities;
using ASP_LABS.Services.BookService;
using ASP_LABS.Services.GenreService;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace ASP_LABS.Areas.Admin
{
	[Authorize]

	public class EditModel : PageModel
    {
        private readonly IBookService _service;
        private readonly IGenreService _genreService;
        private List<Genre> _genreList;
        public EditModel(IBookService service, IGenreService genreService)
        {
            _service = service;
            _genreService = genreService;
            _genreList = _genreService.GetGenreListAsync().Result.Data.Items;
            Genres = new SelectList(_genreList, "Id", "Name");
        }
        [BindProperty]
        public IFormFile? Image { get; set; }

        [BindProperty]
        public Book Book { get; set; } = default!;

        [BindProperty]
        public int GenreId { get; set; }

        public SelectList Genres { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _service.GetBookByIdAsync(id.Value);
            if (!book.Success)
            {
                return NotFound();
            }
            Book = book.Data;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Book.Genre = _genreList.Find(g => g.Id == GenreId);
            Book.GenreId= GenreId; 
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }

            System.Diagnostics.Debug.WriteLine($"--------------------------------------{Image==null}");

            await _service.UpdateBookAsync(Book.Id, Book, Image);


            return RedirectToPage("./Index");
        }

       
    }
}
