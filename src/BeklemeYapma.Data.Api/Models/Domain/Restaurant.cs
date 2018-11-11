using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace BeklemeYapma.Data.Api.Models.Domain
{
    public class Restaurant : DomainBase
    {
        #region Relations

        [BsonElement("companyId")]
        public string CompanyId { get; set; }

        #endregion

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
