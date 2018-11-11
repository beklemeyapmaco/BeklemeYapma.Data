namespace BeklemeYapma.Data.Api.Models.Responses
{
    public class BulkRestaurantCreateResponse
    {
        public bool IsCreated { get; set; }
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
    }
}