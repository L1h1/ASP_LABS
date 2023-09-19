using ASP_LABS.API.Data;
using ASP_LABS.API.Services.BookService;
using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using ASP_LABS.API.Services.BookService;
using Xunit.Abstractions;
using System.Text.Json;

namespace ASP_LABS.Tests
{
    public class ApiBookServiceTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly ITestOutputHelper output;

        AppDbContext CreateContext() => new AppDbContext(_options);

        public ApiBookServiceTests(ITestOutputHelper output)
        {
            this.output = output;

            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

            using var context = CreateContext();
            if (context.Database.EnsureCreated())
                context.Database.EnsureCreated();
            List<Genre> genres = new List<Genre>() { new Genre() { Name = "Thriller" }, new Genre() { Name = "Horror" }, new Genre() { Name = "Science Fiction" }, new Genre() { Name = "Fantasy" } };
            context.AddRange(genres);
            context.SaveChanges();


            context.AddRange(
                new Book() { Title = "FirstTitle", Genre = genres.First(g => g.Name == "Thriller"), Price = 40.99, ImagePath = $"url/images/FirstTitle.jpg" },
                new Book() { Title = "SecondTitle", Genre = genres.First(g => g.Name == "Horror"), Price = 40.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "ThirdTitle", Genre = genres.First(g => g.Name == "Science Fiction"), Price = 40.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "FourthTitle", Genre = genres.First(g => g.Name == "Thriller"), Price = 14.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "FifthTitle", Genre = genres.First(g => g.Name == "Science Fiction"), Price = 74.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "SixthTitle", Genre = genres.First(g => g.Name == "Fantasy"), Price = 24.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "SeventhTitle", Genre = genres.First(g => g.Name == "Fantasy"), Price = 18.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "FirstTitle", Genre = genres.First(g => g.Name == "Thriller"), Price = 40.99, ImagePath = $"url/images/FirstTitle.jpg" },
                new Book() { Title = "SecondTitle", Genre = genres.First(g => g.Name == "Horror"), Price = 40.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "ThirdTitle", Genre = genres.First(g => g.Name == "Science Fiction"), Price = 40.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "FourthTitle", Genre = genres.First(g => g.Name == "Thriller"), Price = 14.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "FifthTitle", Genre = genres.First(g => g.Name == "Science Fiction"), Price = 74.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "SixthTitle", Genre = genres.First(g => g.Name == "Fantasy"), Price = 24.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "SeventhTitle", Genre = genres.First(g => g.Name == "Fantasy"), Price = 18.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "FirstTitle", Genre = genres.First(g => g.Name == "Thriller"), Price = 40.99, ImagePath = $"url/images/FirstTitle.jpg" },
                new Book() { Title = "SecondTitle", Genre = genres.First(g => g.Name == "Horror"), Price = 40.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "ThirdTitle", Genre = genres.First(g => g.Name == "Science Fiction"), Price = 40.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "FourthTitle", Genre = genres.First(g => g.Name == "Thriller"), Price = 14.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "FifthTitle", Genre = genres.First(g => g.Name == "Science Fiction"), Price = 74.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "SixthTitle", Genre = genres.First(g => g.Name == "Fantasy"), Price = 24.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "SeventhTitle", Genre = genres.First(g => g.Name == "Fantasy"), Price = 18.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "FirstTitle", Genre = genres.First(g => g.Name == "Thriller"), Price = 40.99, ImagePath = $"url/images/FirstTitle.jpg" },
                new Book() { Title = "SecondTitle", Genre = genres.First(g => g.Name == "Horror"), Price = 40.99, ImagePath = $"url/images/SecondTitle.jpg" },
                new Book() { Title = "ThirdTitle", Genre = genres.First(g => g.Name == "Science Fiction"), Price = 40.99, ImagePath = $"url/images/SecondTitle.jpg" }
                );

            context.SaveChanges();


        }


        public void Dispose()
        {
            _connection.Dispose();
        }


        [Fact]
        public void DefaultCallTest()
        {
            var env = new Mock<IWebHostEnvironment>();
            var conf = new Mock<IConfiguration>();
            var log = new Mock<Microsoft.Extensions.Logging.ILogger<BookService>>();
            var accessor = new Mock<IHttpContextAccessor>();
            var context = CreateContext();

            var service = new BookService(context, env.Object, conf.Object, log.Object, accessor.Object);

            var data = service.GetBookListAsync("all").Result.Data;

            Assert.Equal(1, data.CurrentPage);
            Assert.Equal(3, data.Items.Count);
            Assert.Equal(8, data.TotalPages);
            Assert.Equal(context.BookSet.First(), data.Items.First());

        }

        [Fact]
        public void CurrentPageTest()
        {
            var env = new Mock<IWebHostEnvironment>();
            var conf = new Mock<IConfiguration>();
            var log = new Mock<Microsoft.Extensions.Logging.ILogger<BookService>>();
            var accessor = new Mock<IHttpContextAccessor>();
            var context = CreateContext();

            var service = new BookService(context, env.Object, conf.Object, log.Object, accessor.Object);

            var data = service.GetBookListAsync("all",2).Result.Data;

            Assert.Equal(2, data.CurrentPage);
            Assert.Equal(context.BookSet.ToList()[3], data.Items.First());

        }

        [Fact]
        public void FilterTest()
        {
            var env = new Mock<IWebHostEnvironment>();
            var conf = new Mock<IConfiguration>();
            var log = new Mock<Microsoft.Extensions.Logging.ILogger<BookService>>();
            var accessor = new Mock<IHttpContextAccessor>();
            var context = CreateContext();

            var service = new BookService(context, env.Object, conf.Object, log.Object, accessor.Object);

            var data = service.GetBookListAsync("science-fiction",1,22).Result.Data;

            Assert.All(data.Items, data => Assert.Equal("science-fiction",data.Genre.NormalizedName));

            data = service.GetBookListAsync("all", 1, 22).Result.Data;
            Assert.Equal(2, data.TotalPages);

        }
        [Fact]
        public void FalsePage()
        {
            var env = new Mock<IWebHostEnvironment>();
            var conf = new Mock<IConfiguration>();
            var log = new Mock<Microsoft.Extensions.Logging.ILogger<BookService>>();
            var accessor = new Mock<IHttpContextAccessor>();
            var context = CreateContext();

            var service = new BookService(context, env.Object, conf.Object, log.Object, accessor.Object);

            var data = service.GetBookListAsync("all", 3, 22).Result;
            Assert.False(data.Success);

        }




    }
}
