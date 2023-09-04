using ASP_LABS.API.Data;
using ASP_LABS.API.Services.BookService;
using ASP_LABS.API.Services.GenreService;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
				x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IGenreService, GenreService>();



var connStr = builder.Configuration.GetConnectionString("SqliteConnection");
string dataDirectory = String.Empty;
connStr = String.Format(connStr, dataDirectory);


var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connStr).Options;
builder.Services.AddScoped<AppDbContext>(s => new AppDbContext(options));


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


await DbInitializer.SeedData(app);


app.Run();

