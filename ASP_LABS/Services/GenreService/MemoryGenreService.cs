using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;

namespace ASP_LABS.Services.GenreService
{
    public class MemoryGenreService : IGenreService
    {
        public Task<ResponseData<ListModel<Genre>>> GetGenreListAsync()
        {
            var genres = new List<Genre>()
            {
                new Genre(){Id=1,Name="Thriller"},
                new Genre(){Id=2,Name="Horror"},
                new Genre(){Id=3,Name="Science Fiction"},
                new Genre(){Id=4,Name="Fantasy"}
            };

            var result = new ResponseData<ListModel<Genre>>() {
                Data = new ListModel<Genre>() 
                {
                    Items = genres
                } 
            };
            return Task.FromResult(result); ;
        }
    }
}
