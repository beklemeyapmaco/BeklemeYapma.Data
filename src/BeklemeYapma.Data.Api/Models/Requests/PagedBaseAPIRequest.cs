using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeklemeYapma.Data.Api.Models.Requests
{
    public class PagedBaseAPIRequest
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
