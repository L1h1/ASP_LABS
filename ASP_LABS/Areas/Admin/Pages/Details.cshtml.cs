﻿using System;
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
    public class DetailsModel : PageModel
    {
        private readonly IBookService _service;

        public DetailsModel(IBookService service)
        {
            _service = service; 
        }

      public Book Book { get; set; } = default!; 

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
            else 
            {
                Book = book.Data;
            }
            return Page();
        }
    }
}