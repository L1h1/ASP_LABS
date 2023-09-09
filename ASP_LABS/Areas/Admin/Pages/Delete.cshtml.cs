using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ASP_LABS.API.Data;
using ASP_LABS.Domain.Entities;
using ASP_LABS.Services.BookService;
using Microsoft.AspNetCore.Authorization;

namespace ASP_LABS.Areas.Admin
{
	[Authorize]

	public class DeleteModel : PageModel
    {
        private readonly IBookService _service;

        public DeleteModel(IBookService service)
        {
            _service = service;
        }

        [BindProperty]
      public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _service.GetBookByIdAsync(id.Value);

            if (book == null)
            {
                return NotFound();
            }
            else 
            {
                Book = book.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _service.GetBookByIdAsync(id.Value);

			if (book != null)
            {
                Book = book.Data;
                await _service.DeleteBookAsync(Book.Id);
                
            }

            return RedirectToPage("./Index");
        }
    }
}
