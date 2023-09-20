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
using ASP_LABS.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASP_LABS.API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookService _service;

        public BookController(IBookService service,HttpClient client)
		{
            _service = service;
        }


		//[AllowAnonymous]
		// GET: api/Book/group/thriller/2/3
		[HttpGet("genre={genre}/page={page}/pageSize={pageSize=3}")]
        public async Task<ResponseData<ListModel<Book>>> GetBookSet(string genre,int page,int pageSize)
        {
            var response = await _service.GetBookListAsync(genre,page,pageSize);
			return response;
        }

		//[AllowAnonymous]
		// GET: api/Book/sample/5
		[HttpGet("id={id}")]
        public async Task<ResponseData<Book>> GetBook(int id)
        {
            var response = await _service.GetBookByIdAsync(id);
			return response;
		}

		// POST: api/Book
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ResponseData<Book>> PostBook([FromBody]Book book)
		{
			System.Diagnostics.Debug.WriteLine($"---------------------------------------------Entered PostBook {book.Title}");
            var response = await _service.CreateBookAsync(book);

            return response;
		}

		// PUT: api/Book/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<ResponseData<Book>> PutBook(int id, [FromBody]Book book)
		{
			var response = new ResponseData<Book>();
            System.Diagnostics.Debug.WriteLine($"---------------------------------------------Entered PutBook {book.Title}");

            if (id != book.Id)
			{

				response.Success = false;
				response.ErrorMessage = "book id conflict";
				return response;
			}

			response = await _service.UpdateBookAsync(id, book);

			return response;
		}

		// DELETE: api/Book/5
		[HttpDelete("{id}")]
		public async Task<ResponseData<bool>> DeleteBook(int id)
		{
			var response = await _service.DeleteBookAsync(id);
			return response;
		}


		// POST: api/Book/5
		[HttpPost("{id}")]
		public async Task<ActionResult<ResponseData<string>>> PostImage(int id,IFormFile formFile)
		{
			var response = await _service.SaveImageAsync(id, formFile);
			if (response.Success)
			{
				return Ok(response);
			}
			return NotFound(response);
		}



	}
}
