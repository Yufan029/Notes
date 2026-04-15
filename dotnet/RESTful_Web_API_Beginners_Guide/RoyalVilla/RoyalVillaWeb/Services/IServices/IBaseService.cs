using RoyalVilla.DTO;
using RoyalVillaWeb.Models;

namespace RoyalVillaWeb.Services.IServices
{
    public interface IBaseService
    {
        Task<T> SendAsync<T>(ApiRequest apiRequest);

        ApiResponse<object> ResponseModel { get; set; }
    }
}
