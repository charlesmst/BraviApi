using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraviApi.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors.Infrastructure;
using BraviApi.Filters;
using BraviApi.Repository;
using BraviApi.Service;

namespace BraviApi
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
            services.AddMvc((options) =>
            {
                options.Filters.Add(typeof(HttpResponseExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connection = Configuration.GetConnectionString("App");

            services.AddDbContext<BraviApiDbContext>(options =>
              options
              .UseSqlServer(connection)
            );

            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IPersonService, PersonService>();

            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IContactService, ContactService>();
            services.AddCors();
            services.AddSwaggerDocument();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
