using ASP_LABS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ASP_LABS.API.Data
{
	public class DbInitializer
	{
		public static async Task SeedData(WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
			string? imagesUrl = app.Configuration.GetSection("ImagesUrl").Get<string>();

			if (string.IsNullOrEmpty(imagesUrl))
			{
				throw new Exception("Missing images url");
			}

			//await context.Database.MigrateAsync();



			System.Diagnostics.Debug.WriteLine($"---------------{context.ContextId}");
			DbSet<Book> books = context.Set<Book>();
			DbSet<Genre> genres = context.Set<Genre>();

			/*await genres.AddAsync(new Genre("Thriller"));
			await genres.AddAsync(new Genre("Horror"));
			await genres.AddAsync(new Genre("Science Fiction"));
			await genres.AddAsync(new Genre("Fantasy"));*/

			await context.SaveChangesAsync();


			/*await books.AddAsync(new Book() { Title = "FirstTitle", Genre = genres.First(g=>g.Name=="Thriller"), Price = 40.99,ImagePath=$"{imagesUrl}/images/FirstTitle.jpg" });
			await books.AddAsync(new Book() { Title = "SecondTitle", Genre = genres.First(g => g.Name == "Horror"), Price = 40.99, ImagePath = $"{imagesUrl}/images/SecondTitle.jpg" });
			await books.AddAsync(new Book() { Title = "ThirdTitle", Genre = genres.First(g => g.Name == "Science Fiction"), Price = 40.99, ImagePath = $"{imagesUrl}/images/ThirdTitle.jpg" });
			await books.AddAsync(new Book() { Title = "FourthTitle", Genre = genres.First(g => g.Name == "Thriller"), Price = 14.99, ImagePath = $"{imagesUrl}/images/FourthTitle.jpg" });
			await books.AddAsync(new Book() { Title = "FifthTitle", Genre = genres.First(g => g.Name == "Science Fiction"), Price = 74.99, ImagePath = $"{imagesUrl}/images/FifthTitle.jpg" });
			await books.AddAsync(new Book() { Title = "SixthTitle", Genre = genres.First(g => g.Name == "Fantasy"), Price = 24.99, ImagePath = $"{imagesUrl}/images/SixthTitle.jpg" });
			await books.AddAsync(new Book() { Title = "SeventhTitle", Genre = genres.First(g => g.Name == "Fantasy"), Price = 18.99, ImagePath = $"{imagesUrl}/images/SeventhTitle.jpg" });
		*/

			await context.SaveChangesAsync();


		}
	}
}
