using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ASP_LABS.Services.GenreService
{
	public class ApiGenreService : IGenreService
	{
		HttpClient _httpClient;
		IConfiguration _configuration;
		ILogger _logger;
		JsonSerializerOptions _serializerOptions;
		public ApiGenreService(HttpClient httpClient, IConfiguration configuration,ILogger<ApiGenreService> logger) 
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_logger = logger;
			_serializerOptions = new JsonSerializerOptions()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
		}
		


		public async Task<ResponseData<ListModel<Genre>>> GetGenreListAsync()
		{
			var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Genre/");
			var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

			if (response.IsSuccessStatusCode)
			{
				try
				{ 
					return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Genre>>>(_serializerOptions);
				}
				catch (JsonException ex)
				{
					_logger.LogError($"-----> Ошибка: {ex.Message}");
					return new ResponseData<ListModel<Genre>>
					{
						Success = false,
						ErrorMessage = $"Ошибка: {ex.Message}"
					};
				}

			}
			_logger.LogError($"-----> Данные не получены от сервера. Error:{ response.StatusCode.ToString()}");
			return new ResponseData<ListModel<Genre>>
			{
				Success = false,
				ErrorMessage = $"Данные не получены от сервера. Error:{ response.StatusCode.ToString() }"
			};
		}
	}
}
