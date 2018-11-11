using System.Collections.Generic;
using System.IO.Compression;
using BeklemeYapma.Data.Api.Filters;
using BeklemeYapma.Data.Api.Services;
using BeklemeYapma.Data.Api.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace BeklemeYapma.Data.Api
{
    public class Startup
    {
        private const string BeklemeYapma_COSMOSDB_DBNAME = "RestaurantDB";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddMvcOptions(o =>
            {
                o.InputFormatters.RemoveType<XmlDataContractSerializerInputFormatter>();
                o.InputFormatters.RemoveType<XmlSerializerInputFormatter>();

                o.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                o.OutputFormatters.RemoveType<StreamOutputFormatter>();
                o.OutputFormatters.RemoveType<StringOutputFormatter>();
                o.OutputFormatters.RemoveType<XmlDataContractSerializerOutputFormatter>();
                o.OutputFormatters.RemoveType<XmlSerializerOutputFormatter>();

                o.Filters.Add<ValidateModelStateFilter>();
                o.Filters.Add<GlobalExceptionFilter>();
            })
            .AddJsonOptions(o =>
            {
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                o.SerializerSettings.Formatting = Formatting.Indented;
                o.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                o.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                o.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression();

            //Services
            services.AddTransient<IRestaurantsService, RestaurantsService>();

            //Data
            services.AddSingleton<IMongoDatabase>(new MongoClient(Configuration.GetValue<string>("BeklemeYapmaCosmosDbUrl")).GetDatabase(BeklemeYapma_COSMOSDB_DBNAME));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "BeklemeYapma.Data.Api", Version = "v1", Description = "Bekleme Yapma - Data API" });
                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
                c.DocumentFilter<TagDescriptionsDocumentFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<Filters.SecurityRequirementsOperationFilter>();

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } },
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeklemeYapma.Data.Api");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = "Bekleme Yapma - Data API";
                c.EnableFilter();
                c.DefaultModelsExpandDepth(-1);
                c.DisplayRequestDuration();
            });

            app.UseResponseCompression();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
