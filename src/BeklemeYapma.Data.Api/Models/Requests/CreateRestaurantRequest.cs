using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BeklemeYapma.Data.Api.Models.Requests
{
    public class CreateRestaurantRequest
    {
        [JsonProperty("company_id")]
        public string CompanyId { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}