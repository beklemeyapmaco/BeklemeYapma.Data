using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;

namespace BeklemeYapma.Data.Api.Models.Domain
{
    public class DomainBase
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}