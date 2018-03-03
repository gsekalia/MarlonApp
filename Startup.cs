using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MarlonApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Cors;


namespace MarlonApi
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
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddMvc();

            // Register the swagger generator defining one or more swagger document
            services.AddSwaggerGen(c =>
             { 
                 c.SwaggerDoc("v1", new Info { Title = "Student API",
                  Version = "v1",
                  Description = "ASP.Net Web API Capstone Project 2018",
                  TermsOfService = "For Class Project Only",
                  Contact = new Contact{ Name = "Adrian Garcia", Email = "tej18359@gmail.com"}
                  });

                  var basePath = AppContext.BaseDirectory;
                  var xmlPath = System.IO.Path.Combine(basePath, "MarlonApi.xml"); 
                  c.IncludeXmlComments(xmlPath); 
             });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                
                app.UseCors("CorsPolicy");
                // Enable middleware to serve generated Swagger as a Json endpoint
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc), specefying the swagger JSON enpoint
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", " My API V1");
                });

            app.UseMvc();

            //app.UseMvc(  routes=>
            //{
            //    routes.MapRoute(
            //        name: "javascript",
            //        template: "javascript/{action}.js",
            //        defaults: new { controller = "mainline" });
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //    routes.MapRoute(
            //        name: "AIMApi",
            //        template: "aim/v1/{action}/{id?}",
            //        defaults: new { controller = "aim" });
            //    routes.MapRoute(
            //        name: "AIMContactsApi",
            //        template: "aim/v1/contacts/{action}/{id?}",
            //        defaults: new { controller = "aimContactsController" }
            //    );
            //    routes.MapRoute(
            //        name: "AIMLocationsApi",
            //        template: "aim/v1/locations/{action}/{id?}",
            //        defaults: new { controller = "aimLocationsController" }
            //    );
            //    routes.MapRoute(
            //        name: "RMSApi",
            //        template: "{controller=rms}/v1/{action}/{id?}");
            //});
            //);

            // http://localhost:5000/swagger/#/
            // http://localhost:5000/swagger/v1/swagger.json
        }
    }
}
