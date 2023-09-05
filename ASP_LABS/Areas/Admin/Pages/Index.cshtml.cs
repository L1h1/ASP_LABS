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

namespace ASP_LABS.Areas.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IBookService _service;

        public IndexModel(IBookService service)
        {
            _service = service;
        }

        public IList<Book> Book { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var response = await _service.GetBookListAsync("all");
            Book = response.Data.Items;


        }
    }
}
