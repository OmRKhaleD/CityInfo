using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Entity;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;

namespace CityInfo.Api
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //to make the application use MVC
            services.AddMvc()
                //allow application return xml result
                .AddMvcOptions(o=>o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
            //make json .net take property name as they as defiend on the class
            //.AddJsonOptions(o=>
            //{
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var contract=o.SerializerSettings.ContractResolver as DefaultContractResolver;
            //        contract.NamingStrategy = null;
            //    }                
            //    });
            services.AddDbContext<CityDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
#if DEBUG
            services.AddTransient<IMailServices,LocalMailServices>();
#else
            services.AddTransient<IMailServices,CloudMailServices>();
#endif
            services.AddScoped<ICityRepository, CityRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory,CityDbContext cityDbContext)
        {
            env.ConfigureNLog("nlog.config");

            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            cityDbContext.EnsureSeedDataForContext();
            //to show http status code in browser
            app.UseStatusCodePages();
            app.UseMvc();
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Entity.City,Models.CityWithoutPointsOfInterest>();
                config.CreateMap<Entity.City, Models.City>();
                config.CreateMap<Entity.PointsOfInterest, Models.PointsOfInterest>();
                config.CreateMap<Models.CreateUpdatePointsOfInterest, Entity.PointsOfInterest>();
                config.CreateMap<Entity.PointsOfInterest, Models.CreateUpdatePointsOfInterest>();
            });
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
