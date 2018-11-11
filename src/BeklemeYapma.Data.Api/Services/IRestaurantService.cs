using BeklemeYapma.Data.Api.Models.Requests;
using BeklemeYapma.Data.Api.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeklemeYapma.Data.Api.Services
{
    public interface IRestaurantService
    {
        Task<BaseResponse<string>> CreateRestaurantAsync(CreateRestaurantRequest request);
        Task<BaseResponse<RestaurantGetResponse>> GetRestaurantAsync(RestaurantGetRequest request);
        Task<BaseResponse<List<RestaurantGetResponse>>> GetRestaurantsAsync(RestaurantGetAllRequest request);
    }
}