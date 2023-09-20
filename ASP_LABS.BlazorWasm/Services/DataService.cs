using ASP_LABS.Domain.Entities;
using ASP_LABS.Domain.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ASP_LABS.BlazorWasm.Services
{
    public class DataService : IDataService
    {

        public List<Genre> Genres { get; set; }
        public List<Book> Books { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        private readonly int apiPageSize;
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _tokenProvider;

        public event Action DataLoaded;

        public DataService(IConfiguration config, HttpClient httpClient, IAccessTokenProvider provider)
        {
            apiPageSize = config.GetValue<int>("ApiRequestParams:PageSize");
            _httpClient= httpClient;
            _tokenProvider = provider;
        }
        
    
        
        public async Task GetCategoryListAsync()
        {
            var httpResponse = await _httpClient.GetAsync("Genre");
            var response = await httpResponse.Content.ReadFromJsonAsync<ResponseData<ListModel<Genre>>>();
            Success = response.Success;
            ErrorMessage = response.ErrorMessage;
            if (Success)
            {
                Genres = response.Data.Items;
            }
			DataLoaded?.Invoke();
		}

        public async Task<ResponseData<Book>> GetProductByIdAsync(int id)
        {
            var httpResponse = await _httpClient.GetAsync($"Book/id={id}");

            var response = await httpResponse.Content.ReadFromJsonAsync<ResponseData<Book>>();
            Success = response.Success;
            ErrorMessage = response.ErrorMessage;
			DataLoaded?.Invoke();
			return response;
        }

        public async Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
            }

            var httpResponse = await _httpClient.GetAsync($"Book/genre={categoryNormalizedName}/page={pageNo}/pageSize={apiPageSize}");
            var response = await httpResponse.Content.ReadFromJsonAsync<ResponseData<ListModel<Book>>>();
            Success = response.Success;
            ErrorMessage = response.ErrorMessage;
            if (Success)
            {
                Books = response.Data.Items;
                TotalPages = response.Data.TotalPages;
                CurrentPage = response.Data.CurrentPage;
            }
            DataLoaded?.Invoke();
        }
    }
}
