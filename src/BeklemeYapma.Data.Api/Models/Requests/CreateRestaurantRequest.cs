using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeklemeYapma.Data.Api.Models.Requests
{
    public class CreateRestaurantRequest
    {
        public string CompanyId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}