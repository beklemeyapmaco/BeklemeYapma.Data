using System.ComponentModel.DataAnnotations;

namespace BeklemeYapma.Data.Api.Models.Requests
{
    public class RestaurantGetAllRequest : PagedBaseAPIRequest
    {
        [Required]
        public string CompanyId { get; set; }
    }
}