using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace PDF2PNG
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.TryAddSingleton<PdfConverterService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "PDF Converter", Version = "v1"});
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //var xmlPath = Path.Combine(basePath, "SampleApi.xml");
                //c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                //c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = "/pdfconverter");
                c.RouteTemplate = "pdfconverter/swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "pdfconverter/swagger";
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Documentation");
                c.SwaggerEndpoint("/pdfconverter/swagger/v1/swagger.json", "V1 Documentation");
            });

            app.UseMvc();
            app.Use(async (context, next) =>
            {
                await next();
                Log.Information("Incoming Request on path {0}, statusCode {1}", context.Request.Path,
                    context.Response.StatusCode);
                await next();
            });
        }
    }
}