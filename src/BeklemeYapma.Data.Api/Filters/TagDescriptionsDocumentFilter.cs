using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BeklemeYapma.Data.Api.Filters
{
    public class TagDescriptionsDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new[] {
                new Tag { Name = "Restaurants", Description = "Browse/manage Restaurants" }
            };
        }
    }
}