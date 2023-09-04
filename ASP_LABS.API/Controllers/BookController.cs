using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP_LABS.API.Data;
using ASP_LABS.Domain.Entities;
using ASP_LABS.API.Services.BookService;
using Azure;

namespace ASP_LABS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        // GET: api/Book/group/thriller/2/3
        [HttpGet("group/{genre}/{page}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookSet(string genre,int page,int pageSize)
        {
            var response = await _service.GetBookListAsync(genre,page,pageSize);
			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return response.Data.Items;
        }

        // GET: api/Book/sample/5
        [HttpGet("sample/{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var response = await _service.GetBookByIdAsync(id);
			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}
			return response.Data;
        }

		// POST: api/Book
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Book>> PostBook(Book book)
		{
			var response = await _service.CreateBookAsync(book);
			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return CreatedAtAction("GetBook", new { id = book.Id }, book);
		}

		// PUT: api/Book/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutBook(int id, Book book)
		{
			if (id != book.Id)
			{
				return BadRequest();
			}

			var response = await _service.UpdateBookAsync(id, book);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return NoContent();
		}


		// DELETE: api/Book/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBook(int id)
		{
			var status = await _service.DeleteBookAsync(id);
			if (!status.Success)
			{
				return NotFound();
			}
			
			return NoContent();
		}

	}
}
