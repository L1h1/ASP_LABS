using ASP_LABS.API.Data;
using ASP_LABS.API.Services.BookService;
using ASP_LABS.API.Services.GenreService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
				x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
;


//L6
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
	opt.Authority = builder
	.Configuration
	.GetSection("isUri").Value;
	opt.IncludeErrorDetails = true;
	opt.TokenValidationParameters.ValidateAudience = false;
	opt.TokenValidationParameters.ValidateIssuer = false;
	opt.TokenValidationParameters.ValidateLifetime = true;
	opt.TokenValidationParameters.ValidateIssuerSigningKey = false;
	opt.TokenValidationParameters.ValidTypes =
	new[] { "at+jwt" };
});
builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddHttpContextAccessor();

var connStr = builder.Configuration.GetConnectionString("SqliteConnection");
string dataDirectory = String.Empty;
connStr = String.Format(connStr, dataDirectory);


var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connStr).Options;
builder.Services.AddScoped<AppDbContext>(s => new AppDbContext(options));




var app = builder.Build();


app.UseCors(opt =>
{
	opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();




//await DbInitializer.SeedData(app);


app.Run();

