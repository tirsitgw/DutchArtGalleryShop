using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.ConfigureServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using AutoMapper;
using System.Reflection;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DutchTreat
{
    public class Startup
    {
    private readonly IConfiguration config;

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public Startup(IConfiguration config)
      {
      this.config = config;
    }

      public void ConfigureServices(IServiceCollection services)
        {
      services.AddIdentity<StoreUser, IdentityRole>(cfg =>
          {
            cfg.User.RequireUniqueEmail = true;
          })
          .AddEntityFrameworkStores<DutchContext>();

          services.AddAuthentication()
                  .AddCookie()
                  .AddJwtBearer(cfg =>
                  {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                      ValidIssuer = config["Tokens:Issuer"],
                      ValidAudience = config["Tokens:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]))
                    };
                  });
            
          services.AddDbContext<DutchContext>(cfg =>
          {
            cfg.UseSqlServer(config.GetConnectionString("DutchConnectionString"));
          });

        
          services.AddTransient<DutchSeeder>();

          services.AddAutoMapper(Assembly.GetExecutingAssembly());

          services.AddScoped<IDutchRepository, DutchRepository>();

          services.AddTransient<IMailServices, NullMailServices>();
      // Support for real mail service



      //services.AddControllersWithViews();
      //        .AddJsonOptions(opt => opt.JsonSerializerOptions.;

      services.AddMvc()
        //.SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
        .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

      



    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/error");
      }

      app.UseStaticFiles();
      app.UseNodeModules();

      
      //moved authorization b/n routing and endpoints

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(cfg =>
      {
        cfg.MapControllerRoute("Fallback",
          "{controller}/{action}/{id?}",
          new { controller = "App", action = "Index" });
      });
     
     


       }
    }
}
