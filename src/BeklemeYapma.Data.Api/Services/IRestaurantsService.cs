using BeklemeYapma.Data.Api.Models.Domain;
using BeklemeYapma.Data.Api.Models.Requests;
using BeklemeYapma.Data.Api.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeklemeYapma.Data.Api.Services
{
    public interface IRestaurantsService
    {
        Task<BaseResponse<string>> CreateRestaurantAsync(CreateRestaurantRequest request);
        Task<BaseResponse<Restaurant>> GetRestaurantAsync(RestaurantGetRequest request);
        Task<BaseResponse<List<Restaurant>>> GetRestaurantsAsync(RestaurantGetAllRequest request);
    }
}