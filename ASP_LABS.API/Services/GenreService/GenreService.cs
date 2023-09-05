using ASP_LABS.API.Data;
using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_LABS.API.Services.GenreService
{
	public class GenreService : IGenreService
	{
		AppDbContext _context;

		public GenreService(AppDbContext context) 
		{
			_context = context;
		}
		public Task<ResponseData<ListModel<Genre>>> GetGenreListAsync()
		{
			var response = new ResponseData<ListModel<Genre>>();
			var data = _context.GenreSet./*Include(g=>g.Books).*/ToList();
			if(data!=null)
				response.Data = new ListModel<Genre>() { Items = data };
			else
			{
				response.Success = false;
				response.ErrorMessage = "Genre section is empty";
			}

			return Task.FromResult(response);
		}
	}
}
