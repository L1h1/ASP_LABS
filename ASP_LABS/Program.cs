using ASP_LABS.API.Data;
using ASP_LABS.Domain.Entities;
using ASP_LABS.Logs;
using ASP_LABS.Models;
using ASP_LABS.Services.BookService;
using ASP_LABS.Services.CartServices;
using ASP_LABS.Services.GenreService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//L6
builder.Services.AddAuthentication(opt =>
{
	opt.DefaultScheme = "cookie";
	opt.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookie")
.AddOpenIdConnect("oidc", options =>
{
	options.Authority =
	builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
	options.ClientId =
	builder.Configuration["InteractiveServiceSettings:ClientId"];
	options.ClientSecret =
	builder.Configuration["InteractiveServiceSettings:ClientSecret"];
	// Получить Claims пользователя
	options.GetClaimsFromUserInfoEndpoint = true;
	options.ResponseType = "code";
	options.ResponseMode = "query";
	options.SaveTokens = true;
	options.Scope.Add("api.read");
	options.Scope.Add("api.write");
});
/*//Adding lab2 services
builder.Services.AddScoped<IGenreService,MemoryGenreService>();
builder.Services.AddScoped<IBookService,MemoryBookService>();
*/


var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();


builder.Services.AddHttpClient<IGenreService, ApiGenreService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<IBookService, ApiBookService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


builder.Services.AddScoped<Cart>(sp=>SessionCart.GetCart(sp));

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning).CreateLogger();

builder.Host.UseSerilog();

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

app.UseSerilogRequestLogging(opts =>
{
	opts.GetLevel = LogHelper.ExcludeHealthChecks;
});

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
	name: "areas",
	  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);
app.MapRazorPages();


app.MapRazorPages().RequireAuthorization();
app.Run();
