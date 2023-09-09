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
using ASP_LABS.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASP_LABS.Areas.Admin
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IBookService _service;

        public IndexModel(IBookService service)
        {
            _service = service;
        }
		[BindProperty]
		public ListModel<Book> Book { get;set; } = default!;

        public async Task OnGetAsync(int pageNo=1)
        {

            var response = await _service.GetBookListAsync("all",pageNo);
            Book = response.Data;


        }
    }
}
