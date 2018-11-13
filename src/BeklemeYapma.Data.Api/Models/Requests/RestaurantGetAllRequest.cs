using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BeklemeYapma.Data.Api.Models.Requests
{
    public class RestaurantGetAllRequest : PagedBaseAPIRequest
    {
        [JsonProperty("company_id")]
        [FromQuery(Name = "company_id")]
        public string CompanyId { get; set; }
    }
}