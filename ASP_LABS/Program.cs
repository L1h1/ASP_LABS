using ASP_LABS.API.Data;
using ASP_LABS.Models;
using ASP_LABS.Services.BookService;
using ASP_LABS.Services.GenreService;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


/*//Adding lab2 services
builder.Services.AddScoped<IGenreService,MemoryGenreService>();
builder.Services.AddScoped<IBookService,MemoryBookService>();
*/


var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();


builder.Services.AddHttpClient<IGenreService, ApiGenreService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<IBookService, ApiBookService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
	name: "areas",
	  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);
app.MapRazorPages();


app.Run();
