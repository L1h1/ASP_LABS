using ASP_LABS.API.Controllers;
using ASP_LABS.API.Data;
using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ASP_LABS.API.Services.BookService
{
	public class BookService : IBookService
	{
		private readonly int _maxPageSize = 20;
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _environment;
		private readonly IConfiguration _config;
		private readonly ILogger<BookService> _logger;
		private readonly IHttpContextAccessor _contextAccessor;

		public BookService(AppDbContext context, IWebHostEnvironment env, IConfiguration configuration, ILogger<BookService> logger,IHttpContextAccessor httpContextAccessor) 
		{
			_context= context;
			_environment = env;
			_config = configuration;
			_logger = logger;
			_contextAccessor = httpContextAccessor;
		}

		public async Task<ResponseData<Book>> CreateBookAsync(Book book)
		{
			var response = new ResponseData<Book>();

			if(_context.BookSet.Any(b=>b.Id == book.Id))
			{
				response.Success = false;
				response.ErrorMessage = "Book with such Id already exists";
				return response;
			}
			book.Genre =await _context.GenreSet.FindAsync(book.GenreId);

            _context.Add<Book>(book);
			_context.SaveChanges();
			response.Data = book;

			return response;
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

		public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
		{
			var response = new ResponseData<string>();
			var book = await _context.BookSet.FindAsync(id);
			if (book == null)
			{
				response.Success = false;
				response.ErrorMessage = "No item found";
				return response;
			}

			var host = "https://" + _contextAccessor.HttpContext.Request.Host;
			var imageFolder = Path.Combine(_environment.WebRootPath, "images");
			if (formFile != null)
			{
				// Удалить предыдущее изображение
				if (!String.IsNullOrEmpty(book.ImagePath))
				{
					var prevImage = Path.GetFileName(book.ImagePath);
					try
					{
						File.Delete(imageFolder + "/" + prevImage);
					}
					catch(Exception ex)
					{
						response.Success = false;
						response.ErrorMessage = ex.Message;
						return response;
					}
				}
				// Создать имя файла
				var ext = Path.GetExtension(formFile.FileName);
				var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);


				

                // Сохранить файл
                string filePath = Path.Combine(imageFolder, fName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                // Указать имя файла в объекте
                book.ImagePath = $"{host}/images/{fName}";

                await _context.SaveChangesAsync();
			}
			response.Data = book.ImagePath;
			return response;


			throw new NotImplementedException();
		}

		public async Task<ResponseData<Book>> UpdateBookAsync(int id, Book book)
		{
			var response = new ResponseData<Book>();
            book.Genre = await _context.GenreSet.FindAsync(book.GenreId);


            if (!_context.BookSet.Any(b => b.Id == book.Id))
			{
				response.Success = false;
				response.ErrorMessage = "Nothing to update";
				return response;
			}
			_context.Entry(book).State = EntityState.Modified;

			_context.Update<Book>(book);
			_context.SaveChanges();
			response.Data = book;
			return response;
		}
	}
}
