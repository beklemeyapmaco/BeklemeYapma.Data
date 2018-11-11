using BeklemeYapma.Data.Api.Models.Domain;
using BeklemeYapma.Data.Api.Models.Requests;
using BeklemeYapma.Data.Api.Models.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeklemeYapma.Data.Api.Services.Implementations
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly ILogger<RestaurantsService> _logger;
        private readonly IConfiguration _configuration;
        public readonly IMongoDatabase _mongoDatabase;
        private const string BEKLEMEYAPMA_RESTAURANT_COLLECTION_NAME = "Restaurants";

        public RestaurantsService(ILogger<RestaurantsService> logger, IConfiguration configuration, IMongoDatabase mongoDatabase)
        {
            _logger = logger;
            _configuration = configuration;
            _mongoDatabase = mongoDatabase;
        }

        public async Task<BaseResponse<string>> CreateRestaurantAsync(CreateRestaurantRequest request)
        {
            BaseResponse<string> createRestaurantResponse = new BaseResponse<string>();

            try
            {
                (bool, string) isStructureValidaded = ValidateRequestStructure(request);

                if (isStructureValidaded.Item1)
                {
                    var collection = _mongoDatabase.GetCollection<Restaurant>(BEKLEMEYAPMA_RESTAURANT_COLLECTION_NAME);

                    var document = new Restaurant
                    {
                        Name = request.Name,
                        CompanyId = request.CompanyId
                    };

                    await collection.InsertOneAsync(document);

                    createRestaurantResponse.Data = document.Id;
                }
                else
                {
                    createRestaurantResponse.Errors.Add(isStructureValidaded.Item2);
                }
            }
            catch (Exception ex)
            {
                createRestaurantResponse.Errors.Add(ex.Message);

                _logger.LogError(ex, ex.Message);
            }

            return createRestaurantResponse;
        }

        public async Task<BaseResponse<Restaurant>> GetRestaurantAsync(RestaurantGetRequest request)
        {
            BaseResponse<Restaurant> response = new BaseResponse<Restaurant>();

            try
            {
                IAsyncCursor<Restaurant> Restaurants = await _mongoDatabase.GetCollection<Restaurant>(BEKLEMEYAPMA_RESTAURANT_COLLECTION_NAME).FindAsync(d => d.Id == request.Id);

                response.Data = await Restaurants.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                _logger.LogError(ex, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<List<Restaurant>>> GetRestaurantsAsync(RestaurantGetAllRequest request)
        {
            BaseResponse<List<Restaurant>> getRestaurantsResponse = new BaseResponse<List<Restaurant>>();

            try
            {
                var filters = new List<FilterDefinition<Restaurant>>();
                var builder = Builders<Restaurant>.Filter;

                if (!string.IsNullOrEmpty(request.CompanyId))
                    filters.Add(builder.Eq(r => r.CompanyId, request.CompanyId));


                FilterDefinition<Restaurant> query = filters.Any()
                    ? builder.And(filters)
                    : builder.Empty;

                Task<List<Restaurant>> Restaurants = _mongoDatabase.GetCollection<Restaurant>(BEKLEMEYAPMA_RESTAURANT_COLLECTION_NAME)
                                                    .Find(query)
                                                    .Skip(request.Offset)
                                                    .Limit(request.Limit)
                                                    .ToListAsync();

                Task<long> count = _mongoDatabase.GetCollection<Restaurant>(BEKLEMEYAPMA_RESTAURANT_COLLECTION_NAME)
                    .CountDocumentsAsync(query);

                await Task.WhenAll(Restaurants, count);

                getRestaurantsResponse.Total = Convert.ToInt32(count.Result);
                getRestaurantsResponse.Data = Restaurants.Result;
            }
            catch (Exception ex)
            {
                getRestaurantsResponse.Errors.Add(ex.Message);
                _logger.LogError(ex, ex.Message);
            }

            return getRestaurantsResponse;
        }

        private (bool, string) ValidateRequestStructure(CreateRestaurantRequest request)
        {
            string[] requiredFieldsForRestaurantStructure = _configuration.GetValue<string>("RequiredFieldsForRestaurantStructure").Split(',').ToArray();

            for (int i = 0; i < requiredFieldsForRestaurantStructure.Length; i++)
            {
                var prop = request.GetType().GetProperty(requiredFieldsForRestaurantStructure[i]);
                var value = prop.GetValue(request);

                if (prop == null)
                {
                    return (false, $"The {prop.Name} value cannot be null.");
                }
            }

            return (true, null);
        }
    }
}