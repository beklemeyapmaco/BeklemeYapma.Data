using System.ComponentModel.DataAnnotations;

namespace BeklemeYapma.Data.Api.Models.Requests
{
    public class RestaurantGetAllRequest : PagedBaseAPIRequest
    {
        public string CompanyId { get; set; }
    }
}