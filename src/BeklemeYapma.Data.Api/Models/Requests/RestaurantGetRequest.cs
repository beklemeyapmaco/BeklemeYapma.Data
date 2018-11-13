using Newtonsoft.Json;

namespace BeklemeYapma.Data.Api.Models.Requests
{
    public class RestaurantGetRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}