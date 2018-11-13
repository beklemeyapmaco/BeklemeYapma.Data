using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BeklemeYapma.Data.Api.Models.Requests
{
    public class PagedBaseAPIRequest
    {
        [JsonProperty("offset")]
        [FromQuery(Name = "offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        [FromQuery(Name = "limit")]
        public int Limit { get; set; }
    }
}
