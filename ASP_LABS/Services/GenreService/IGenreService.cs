using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;

namespace ASP_LABS.Services.GenreService
{
    public interface IGenreService
    { 
        public Task<ResponseData<ListModel<Genre>>> GetGenreListAsync();
    }
}
