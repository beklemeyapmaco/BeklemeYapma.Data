using BeklemeYapma.Data.Api.Models.Domain;
using BeklemeYapma.Data.Api.Models.Requests;
using BeklemeYapma.Data.Api.Models.Responses;
using BeklemeYapma.Data.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BeklemeYapma.Data.Api.Controllers
{
    [Route("api/Restaurants")]
    [ApiController]
    public class RestaurantsController : BaseController
    {
        private readonly IRestaurantsService _RestaurantService;

        public RestaurantsController(IRestaurantsService RestaurantService)
        {
            _RestaurantService = RestaurantService;
        }

        [HttpPost("batch")]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(List<BulkRestaurantCreateResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Request not accepted.")]
        public async Task<IActionResult> Post([FromBody]List<CreateRestaurantRequest> requests)
        {
            if (requests.Count > 0)
            {
                List<BulkRestaurantCreateResponse> bulkRestaurantCreateResponse = new List<BulkRestaurantCreateResponse>();

                foreach (var request in requests)
                {
                    BaseResponse<string> createRestaurantResponse = await _RestaurantService.CreateRestaurantAsync(request);

                    bulkRestaurantCreateResponse.Add(new BulkRestaurantCreateResponse
                    {
                        IsCreated = !string.IsNullOrEmpty(createRestaurantResponse.Data),
                        RestaurantId = createRestaurantResponse.Data,
                        RestaurantName = request.Name
                    });
                }

                return Created("", bulkRestaurantCreateResponse);
            }
            else
            {
                return BadRequest("Request not accepted.");
            }
        }

        [HttpPost()]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Request not accepted.")]
        public async Task<IActionResult> Post([FromBody]CreateRestaurantRequest request)
        {
            BaseResponse<string> createRestaurantResponse = await _RestaurantService.CreateRestaurantAsync(request);

            if (createRestaurantResponse.HasError)
            {
                return BadRequest(createRestaurantResponse.Errors);
            }
            else
            {
                return Created("", new { id = createRestaurantResponse.Data });
            }
        }

        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Restaurant))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "No Restaurant found for requested filter.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Request not accepted.")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                RestaurantGetRequest request = new RestaurantGetRequest { Id = id };
                BaseResponse<Restaurant> RestaurantResponse = await _RestaurantService.GetRestaurantAsync(request);

                if (RestaurantResponse.HasError)
                {
                    return BadRequest(RestaurantResponse.Errors);
                }

                if (RestaurantResponse.Data == null)
                {
                    return NotFound("No Restaurant found for requested filter.");
                }

                return Ok(RestaurantResponse.Data);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<RestaurantGetAllRequest>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "No Restaurant found for requested filter.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Request not accepted.")]
        public async Task<IActionResult> Get([FromQuery]RestaurantGetAllRequest request)
        {
            BaseResponse<List<Restaurant>> RestaurantGetAllResponse = await _RestaurantService.GetRestaurantsAsync(request);

            if (RestaurantGetAllResponse.HasError)
            {
                return BadRequest(RestaurantGetAllResponse.Errors);
            }

            if (RestaurantGetAllResponse.Data == null || RestaurantGetAllResponse.Data.Count == 0)
            {
                return NotFound("No Restaurant found for requested filter.");
            }

            var response = new PagedAPIResponse<List<Restaurant>>();
            response.Items = new List<Restaurant>();
            response.Items.AddRange(RestaurantGetAllResponse.Data);

            PreparePagination(request.Offset, request.Limit, RestaurantGetAllResponse.Total, "Restaurants", response);

            return Ok(response);
        }
    }
}