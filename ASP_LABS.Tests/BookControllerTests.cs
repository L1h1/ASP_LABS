using ASP_LABS.API.Data;
using ASP_LABS.Controllers;
using ASP_LABS.Services.BookService;
using ASP_LABS.Services.GenreService;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;
using Xunit.Abstractions;

namespace ASP_LABS.Tests
{
    public class BookControllerTests 
    {
        private readonly ITestOutputHelper output;
        public BookControllerTests(ITestOutputHelper output) {  this.output = output; }


        [Fact]
        public void ErrorGenre()
        {
            var bookService = new Mock<IBookService>();
            var genreService = new Mock<IGenreService>();

            var data = Task.FromResult(new ResponseData<ListModel<Genre>>() { Data = new(), Success = false, ErrorMessage = "GENRE SAMPLE ERROR MESSAGE" });
            genreService.Setup(m => m.GetGenreListAsync()).Returns(()=>data);

            BookController controller = new BookController(bookService.Object,genreService.Object);

            // Act
            var result = controller.Index("all", 1).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("GENRE SAMPLE ERROR MESSAGE", (result as NotFoundObjectResult).Value);

        }

        [Fact]
        public void ErrorBook()
        {
            var bookService = new Mock<IBookService>();
            var genreService = new Mock<IGenreService>();

            
            var genreData = Task.FromResult(new ResponseData<ListModel<Genre>>() { Data = new(), Success = true, ErrorMessage = "" });
            genreService.Setup(m => m.GetGenreListAsync()).Returns(() => genreData);

            var bookData = Task.FromResult(new ResponseData<ListModel<Book>>() { Data = new(), Success = false, ErrorMessage = "BOOK SAMPLE ERROR MESSAGE" });
            bookService.Setup(m=>m.GetBookListAsync(It.IsAny<string>(),It.IsAny<int>())).Returns(()=>bookData);


            BookController controller = new BookController(bookService.Object, genreService.Object);

            // Act
            var result = controller.Index("all", 1).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("BOOK SAMPLE ERROR MESSAGE", (result as NotFoundObjectResult).Value);

        }

        [Fact]
        public void ViewGenres()
        {
            var bookService = new Mock<IBookService>();
            var genreService = new Mock<IGenreService>();


            var genreData = Task.FromResult(new ResponseData<ListModel<Genre>>() { Data =new ListModel<Genre>() {Items = new List<Genre>() { new Genre() {Name="Some name" } } }, Success = true, ErrorMessage = "GENRE SAMPLE ERROR MESSAGE" });
            genreService.Setup(m => m.GetGenreListAsync()).Returns(() => genreData);

            var bookData = Task.FromResult(new ResponseData<ListModel<Book>>() { Data = new ListModel<Book>() { Items = new List<Book>() { new Book() { Title = "Some name" } } }, Success = true, ErrorMessage = "BOOK SAMPLE ERROR MESSAGE" });
            bookService.Setup(m => m.GetBookListAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(() => bookData);


            BookController controller = new BookController(bookService.Object, genreService.Object);

            // Act
            var result = controller.Index("all", 1).Result;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(genreData.Result.Data,(result as ViewResult).ViewData["Genres"]);
            Assert.Equal("Выберите жанр", (result as ViewResult).ViewData["CurrentGenre"]);
            Assert.Equal(bookData.Result.Data, (result as ViewResult).Model);
        }

    }
}
