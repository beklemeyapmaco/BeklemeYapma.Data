using Newtonsoft.Json;

namespace BeklemeYapma.Data.Api.Models.Responses
{
    public class BulkRestaurantCreateResponse
    {
        [JsonProperty("is_created")]
        public bool IsCreated { get; set; }

        [JsonProperty("restaurant_id")]
        public string RestaurantId { get; set; }

        [JsonProperty("restaurant_name")]
        public string RestaurantName { get; set; }
    }
}