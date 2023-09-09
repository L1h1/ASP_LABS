using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;
using ASP_LABS.Services.GenreService;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASP_LABS.Services.BookService
{
	public class ApiBookService : IBookService
	{
		HttpClient _httpClient;
		IConfiguration _configuration;
		ILogger _logger;
		string _pageSize;
		JsonSerializerOptions _serializerOptions;
		HttpContext _httpContext;

		public ApiBookService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiGenreService> logger, IHttpContextAccessor httpContextAccessor)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_logger = logger;
			_pageSize = _configuration.GetSection("ItemsPerPage").Value;

			_serializerOptions = new JsonSerializerOptions()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			_httpContext = httpContextAccessor.HttpContext;

		}

		public async Task DeleteBookAsync(int id)
		{
			var token = await _httpContext.GetTokenAsync("access_token");
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
			System.Diagnostics.Debug.WriteLine(token);

			var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Book/");
			urlString.Append($"{id}");
			var response = await _httpClient.DeleteAsync(urlString.ToString());
			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError($"----->Книга с таким Id отсутствует. Error:{response.StatusCode}");
			}
		}

		public async Task<ResponseData<Book>> GetBookByIdAsync(int id)
		{
			var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Book/");
			urlString.Append($"id={id}/");

			var response = await _httpClient.GetAsync(urlString.ToString());

			if (response.IsSuccessStatusCode)
			{
				try
				{
					return await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);
				}
				catch (JsonException ex)
				{
					_logger.LogError($"-----> Ошибка: {ex.Message}");
					return new ResponseData<Book>
					{
						Success = false,
						ErrorMessage = $"Ошибка: {ex.Message}"
					};
				}

			}
			_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode.ToString()}");
			return new ResponseData<Book>
			{
				Success = false,
				ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode.ToString()}"
			};

			throw new NotImplementedException();
		}

		public async Task<ResponseData<ListModel<Book>>> GetBookListAsync(string? bookNormalizedName, int pageNo = 1)
		{
			var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Book/");

			urlString.Append($"genre={bookNormalizedName}/");
			urlString.Append($"page={pageNo}/");
			urlString.Append($"pageSize={_pageSize}/");

			var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

			if (response.IsSuccessStatusCode)
			{
				try
				{
					return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Book>>>(_serializerOptions);
				}
				catch (JsonException ex)
				{
					_logger.LogError($"-----> Ошибка: {ex.Message}");
					return new ResponseData<ListModel<Book>>
					{
						Success = false,
						ErrorMessage = $"Ошибка: {ex.Message}"
					};
				}

			}
			_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode.ToString()}");
			return new ResponseData<ListModel<Book>>
			{
				Success = false,
				ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode.ToString()}"
			};
		}
		private async Task SaveImageAsync(int id, IFormFile image)
		{
			var token = await _httpContext.GetTokenAsync("access_token");
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Book/{id}")
			};
			System.Diagnostics.Debug.WriteLine("-------------------------------------------Save img async");
			var content = new MultipartFormDataContent();
			var streamContent = new StreamContent(image.OpenReadStream());
			content.Add(streamContent, "formFile", image.FileName);
			request.Content = content;
			await _httpClient.SendAsync(request);
		}

		public async Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile)
		{
			var token = await _httpContext.GetTokenAsync("access_token");
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

			var RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Book");

            var response = await _httpClient.PostAsJsonAsync(RequestUri, book);

            var responseData = await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);

			if (!responseData.Success)
			{
				throw new Exception(responseData.ErrorMessage);
			}

			if(formFile != null)
				await SaveImageAsync(responseData.Data.Id,formFile);
			return responseData;


		}



		public async Task UpdateBookAsync(int id, Book book, IFormFile? formFile)
		{
			var token = await _httpContext.GetTokenAsync("access_token");
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

			var RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Book/{id}");
            var response = await _httpClient.PutAsJsonAsync(RequestUri, book, _serializerOptions);

            var responseData = await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);
            if (!responseData.Success)
            {
                throw new Exception(responseData.ErrorMessage);
            }
            if (formFile != null)
			{
				System.Diagnostics.Debug.WriteLine("---------------------------------recieved Image");
                await SaveImageAsync(responseData.Data.Id, formFile);
            }
				
		}
	}
}
