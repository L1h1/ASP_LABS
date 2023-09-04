using ASP_LABS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP_LABS.API.Data
{
	public class AppDbContext : DbContext
	{

		public DbSet<Book> BookSet { get; set; }
		public DbSet<Genre> GenreSet { get; set; }
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			this.ChangeTracker.LazyLoadingEnabled = false; //anti-recursion
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}


	}
}
