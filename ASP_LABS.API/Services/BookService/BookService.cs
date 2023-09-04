using ASP_LABS.API.Data;
using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_LABS.API.Services.BookService
{
	public class BookService : IBookService
	{
		private readonly int _maxPageSize = 20;
		private AppDbContext _context;

		public BookService(AppDbContext context) 
		{
			_context= context;
		}

		public Task<ResponseData<Book>> CreateBookAsync(Book book)
		{
			var response = new ResponseData<Book>();
			
			var set = _context.BookSet;
			if(set.Any(b=>b.Id == book.Id))
			{
				response.Success = false;
				response.ErrorMessage = "Book with such Id already exists";
				return Task.FromResult(response);
			}

			set.Add(book);
			_context.SaveChanges();

			return Task.FromResult(response);
		}

		public Task<ResponseData<bool>> DeleteBookAsync(int id)
		{
			var response = new ResponseData<bool>();
			var result = _context.BookSet.Find(id);

			if (result == null)
			{
				response.Success = false;
				response.ErrorMessage = "book not found";
				return Task.FromResult(response);

			}
				
			_context.BookSet.Remove(result);
			_context.SaveChanges();

			return Task.FromResult(response);
		}

		public Task<ResponseData<Book>> GetBookByIdAsync(int id)
		{
			var response = new ResponseData<Book>();
			var result = _context.BookSet.Include(b => b.Genre).ToList().Find(b=>b.Id==id);

			if (result == null)
			{
				response.Success = false;
				response.ErrorMessage = "No such book id";
			}
			response.Data=result;
			return Task.FromResult(response);

		}

		public Task<ResponseData<ListModel<Book>>> GetBookListAsync(string? genreNormalizedName, int pageNo = 1, int pageSize = 3)
		{
			var response = new ResponseData<ListModel<Book>>();
			response.Data = new ListModel<Book>();

			if (pageSize > _maxPageSize)
				pageSize = _maxPageSize;


			var query = _context.BookSet.Include(b=>b.Genre).AsQueryable();
			if (genreNormalizedName != "all")
				query = query.Where(b => b.Genre.NormalizedName!.Equals(genreNormalizedName));

			int totalPages = (int)Math.Ceiling(query.Count() / (double)pageSize);


			if (pageNo > totalPages)
			{
				response.Success = false;
				response.ErrorMessage = "No such page";
				return Task.FromResult(response);
			}

			response.Data.Items=query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			response.Data.CurrentPage = pageNo;
			response.Data.TotalPages = totalPages;


			return Task.FromResult(response);

		}

		public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
		{
			var response = new ResponseData<string>();

			if (formFile == null)
			{
				response.Success = false;
				response.ErrorMessage = "no image provided";
				return Task.FromResult(response);
			}
			if (!_context.BookSet.Any(b => b.Id == id))
			{
				response.Success = false;
				response.ErrorMessage = "no such book";
				return Task.FromResult(response);
			}


			string path =$"/images/{_context.BookSet.Find(id).Title}.jpg";

			//?????





			throw new NotImplementedException();
		}

		public Task<ResponseData<Book>> UpdateBookAsync(int id, Book book)
		{
			var response = new ResponseData<Book>();

			var set = _context.BookSet;
			if (!set.Any(b => b.Id == book.Id))
			{
				response.Success = false;
				response.ErrorMessage = "Nothing to update";
				return Task.FromResult(response);
			}
			_context.Entry(book).State = EntityState.Modified;

			set.Update(book);
			_context.SaveChanges();

			return Task.FromResult(response);
		}
	}
}
