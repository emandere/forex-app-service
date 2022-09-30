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
using forex_app_service.Middleware;

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
                options.ForexAccount 
                    = Configuration.GetSection("Account:Account").Value;  
                options.Token 
                    = Configuration.GetSection("Account:Token").Value;
                options.URL 
                    = Configuration.GetSection("URL").Value;
                options.AllowUpdates
                    = Configuration.GetSection("AccessControl").GetValue<bool>("AllowUpdates");
            });

            
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //services.AddMvc(option => option.EnableEndpointRouting = true);
           
            services.AddControllers();
             var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ForexPriceProfile());
                cfg.AddProfile(new ForexPriceIndicatorProfile());
                cfg.AddProfile(new ForexSessionProfile());
            });

            var profiles = new List<Profile>{
                new ForexPriceProfile(),
                new ForexDailyPriceProfile(),
                new ForexPriceIndicatorProfile(), 
                new ForexSessionProfile()   
            };
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
                    //builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    builder.WithOrigins("http://localhost:4200",
                    "http://forex.localdev.me:8080/",
                    "http://forex.enappsaws.net/",
                    "https://forex.localdev.me:8443/",
                    "https://forex.enappsaws.net/")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
   

                    /*var origins = Configuration.GetSection("CORS:origins").GetChildren();
                    foreach(var origin in origins)
                    {
                        builder.WithOrigins(origin.Value).AllowAnyHeader().AllowAnyMethod();
                    }*/

                });
            });

            //IMapper mapper = config.CreateMapper();
            services.AddTransient<ForexPriceMap,ForexPriceMap>();
            services.AddTransient<ForexDailyPriceMap,ForexDailyPriceMap>();
            services.AddTransient<ForexPriceIndicatorMap,ForexPriceIndicatorMap>();
            services.AddTransient<ForexIndicatorMap,ForexIndicatorMap>();
            services.AddTransient<ForexSessionMap,ForexSessionMap>();
            services.AddTransient<ForexRuleMap,ForexRuleMap>();
            services.AddTransient<ForexTradeMap,ForexTradeMap>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            //services.AddSingleton(mapper);
            services.AddAutoMapper(c=>c.AddProfiles(profiles),typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName=="Development")
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAllowUpdates();

            //app.UseHttpsRedirection();
            
            //app.UseMvc();
            app.UseRouting(); 
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                // Mapping of endpoints goes here:
                endpoints.MapControllers();
            });
        }
    }
}
