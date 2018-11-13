using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BeklemeYapma.Data.Api.Models.Domain
{
    public class Restaurant : DomainBase
    {
        #region Relations

        [BsonElement("companyId")]
        [JsonProperty("company_id")]
        public string CompanyId { get; set; }

        #endregion

        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
