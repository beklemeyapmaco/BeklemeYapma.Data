using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeklemeYapma.Data.Api.Models.Responses
{
    public class PagedAPIResponse<TItems>
    {
        public long Index { get; set; }
        public long PageSize { get; set; }
        public long? Total { get; set; }
        public TItems Items { get; set; }
        public string First { get; set; }
        public string Next { get; set; }
        public string Prev { get; set; }
        public string Last { get; set; }
    }
}
