using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace CityInfo.Api
{
    public class Startup
    {
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //to show http status code in browser
            app.UseStatusCodePages();
            app.UseMvc();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
