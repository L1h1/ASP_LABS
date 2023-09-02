﻿using ASP_LABS.Domain;
using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;
using ASP_LABS.Services.GenreService;
using Microsoft.AspNetCore.Mvc;

namespace ASP_LABS.Services.BookService
{
    public class MemoryBookService : IBookService
    {
        IGenreService _genreService;
        List<Book> _books;
        List<Genre> _genres;
        IConfiguration _configuration;

        public MemoryBookService([FromServices] IConfiguration config,IGenreService genreService)
        {
            _genreService = genreService;
            _genres = _genreService.GetGenreListAsync().Result.Data.Items;
            _configuration = config;
            SetData();
        }

        private void SetData()
        {
            _books = new List<Book>()
            {
                new Book() {Id=1,Title="FirstTitle",Genres=new List<Genre>(){_genres[0] },Price=40.99 },
                new Book() {Id=2,Title="SecondTitle",Genres=new List<Genre>(){_genres[1]},Price=20.00 },
                new Book() {Id=3,Title="ThirdTitle",Genres=new List<Genre>(){_genres[2]},Price=69.99 },
				new Book() {Id=4,Title="FourthTitle",Genres=new List<Genre>(){_genres[3]},Price=14.99 },
				new Book() {Id=5,Title="FifthTitle",Genres=new List<Genre>(){_genres[2],_genres[0]},Price=74.99 },
				new Book() {Id=6,Title="SixthTitle",Genres=new List<Genre>(){_genres[0],_genres[1]},Price=24.99 },
				new Book() {Id=7,Title="SeventhTitle",Genres=new List<Genre>(){_genres[3],_genres[1]},Price=18.99 }
			};
        }

        public Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Book>>> GetBookListAsync(string? genreNormalizedName, int pageNo = 1)
        {
            var result = new ListModel<Book>();
            //размер страницы
            int itemsPerPage = _configuration.GetSection("ItemsPerPage").Get<int>();
            


            if (genreNormalizedName == "all")
                result.Items = _books;
            else
            {
				result.Items= _books.Where(b=>b.Genres.Any(g=>g.NormalizedName == genreNormalizedName)).ToList();
			}
			int itemCount = result.Items.Count;

			//общее число страниц
			int pages = itemCount % 3 == 0 ? itemCount / 3 : itemCount / 3 + 1;


            result.Items = result.Items.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList();


			return Task.FromResult(new ResponseData<ListModel<Book>>() {
                Data= result 
            });
        }

        public Task UpdateBookAsync(int id, Book book, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}