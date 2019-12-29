using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;

using forex_app_service.Models;
using forex_app_service.Domain;
using forex_app_service.Config;
using forex_app_service.Mapper;

namespace forex_app_service
{
    //Pipeline Tests
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Settings>(options =>
            {
                options.ConnectionString 
                    = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database 
                    = Configuration.GetSection("MongoConnection:Database").Value;
                options.ForexAppServiceBase
                    = Configuration.GetSection("ForexAppService:Base").Value;    
            });
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMvc(option => option.EnableEndpointRouting = false);
             var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ForexPriceProfile());
                cfg.AddProfile(new ForexPriceIndicatorProfile());
                cfg.AddProfile(new ForexSessionProfile());
            });
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    var origins = Configuration.GetSection("CORS:origins").GetChildren();
                    foreach(var origin in origins)
                    {
                        builder.WithOrigins(origin.Value);
                    }

                });
            });

            IMapper mapper = config.CreateMapper();
            services.AddTransient<ForexPriceMap,ForexPriceMap>();
            services.AddTransient<ForexPriceIndicatorMap,ForexPriceIndicatorMap>();
            services.AddTransient<ForexIndicatorMap,ForexIndicatorMap>();
            services.AddTransient<ForexSessionMap,ForexSessionMap>();
            services.AddSingleton(mapper);
            //services.AddAutoMapper();
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

            //app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseMvc();
        }
    }
}
