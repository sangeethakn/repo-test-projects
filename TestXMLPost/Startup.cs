using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace TestXMLPost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
               .AddSwaggerGen(c =>
               {
                   c.SwaggerDoc("v1", new Info { Title = "Test API", Version = "v1" });
                   c.CustomSchemaIds(i => i.FullName);
               });
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
                options.InputFormatters.Insert(0,new XDocumentInputFormatter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                 .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI((option) =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API");
                option.DocumentTitle = "Test API";
            });


            logger.AddSerilog();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
