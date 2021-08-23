using IsatiIntegration.API.Settings;
using IsatiIntegration.API.Settings.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API
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
            // Configuration part
            services.Configure<IsatiIntegrationSettings>(Configuration.GetSection(nameof(IsatiIntegrationSettings)));
            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));

            services.AddSingleton<IIsatiIntegrationSettings>(Span => Span.GetRequiredService<IOptions<IsatiIntegrationSettings>>().Value);
            services.AddSingleton<IMongoSettings>(Span => Span.GetRequiredService<IOptions<IMongoSettings>>().Value);

            // Services part

            // Cors policy
            services.AddCors(options =>
            {
                options.AddPolicy("developerPolicy", builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials();
                });
            });

            // Controllers
            services.AddControllers();

            // Swagger documentation part
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "IsatiIntegration.API",
                    Version = "v1",
                    Description = "The API of the application for the ESIR integration month",
                    Contact = new OpenApiContact
                    {
                        Name = "Victor (Feldrise) DENIS",
                        Email = "cto@isati.org",
                        Url = new Uri("https://isati.org")
                    }
                });

                var filepath = Path.Combine(System.AppContext.BaseDirectory, "IsatiIntegration.API.xml");
                c.IncludeXmlComments(filepath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IsatiIntegration.API v1");
                c.InjectStylesheet("/swagger/themes/theme-newspaper.css");
                c.InjectJavascript("/swagger/custom-script.js", "text/javascript");
                c.RoutePrefix = "documentation";
            });

            app.UseCors("developerPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
