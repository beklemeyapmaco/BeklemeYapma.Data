using System.Collections.Generic;
using System.Linq;

namespace BeklemeYapma.Data.Api.Models.Responses
{
    public class BaseResponse<TData>
    {
        public BaseResponse()
        {
            Errors = new List<string>();
        }

        public TData Data { get; set; }
        public bool HasError => Errors.Any();
        public List<string> Errors { get; set; }
        public int Total { get; set; }
    }
}