using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace RoyalVillaWeb.Services
{
    public class BaseService : IBaseService
    {
        public IHttpClientFactory HttpClient { get; set; }
        public ApiResponse<object> ResponseModel { get; set; } = new ApiResponse<object>();

        private static readonly JsonSerializerOptions JsonOption = new ()
        {
            PropertyNameCaseInsensitive = true
        };

        public BaseService(IHttpClientFactory httpClient)
        {
            this.ResponseModel = new();
            this.HttpClient = httpClient;
        }

        public async Task<T?> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = HttpClient.CreateClient("RoyalVillaAPI");
                var message = new HttpRequestMessage
                {
                    RequestUri = new Uri(apiRequest.Url!, uriKind: UriKind.Relative),
                    Method = GetHttpMethod(apiRequest.ApiType)
                };

                if (apiRequest.Data != null)
                {
                    message.Content = JsonContent.Create(apiRequest.Data, options: JsonOption);
                }

                var apiResponse = await client.SendAsync(message);

                return await apiResponse.Content.ReadFromJsonAsync<T>(JsonOption);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return default;
            }
        }

        private static HttpMethod GetHttpMethod(SD.ApiType apiType)
        {
            return apiType.ToString().ToUpper() switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                _ => HttpMethod.Get
            };
        }
    }
}
