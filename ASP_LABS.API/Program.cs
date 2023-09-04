using ASP_LABS.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connStr = builder.Configuration.GetConnectionString("SqliteConnection");
System.Diagnostics.Debug.WriteLine(connStr);
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

app.Run();
