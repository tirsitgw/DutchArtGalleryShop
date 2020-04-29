using AutoMapper.Mappers.Internal;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
  public class DutchSeeder
  {
    private readonly DutchContext ctx;
    private readonly IWebHostEnvironment webHost;
    private readonly UserManager<StoreUser> userManager;

    //private readonly IHostingEnvironment hosting;

    public DutchSeeder(DutchContext ctx, IWebHostEnvironment webHost, UserManager<StoreUser> userManager)
    {
      this.ctx = ctx;
      this.webHost = webHost;
      this.userManager = userManager;
      // this.hosting = hosting;
    }

    public async Task SeedAsync()
    {
      ctx.Database.EnsureCreated();

      StoreUser user = await userManager.FindByEmailAsync("eth.tirsit@gmail.com");
      if(user == null)
      {
        user = new StoreUser()
        {
          FirstName = "Tirsit",
          LastName = "Getu",
          Email = "eth.tirsit@gmail.com",
          UserName = "eth.tirsit@gmail.com"
        };

        var result = await userManager.CreateAsync(user, "P@ssw0rd!");
        if(result!= IdentityResult.Success)
        {
          throw new InvalidOperationException("Could not create new user in seeder");
        }
      }

      if(!ctx.Products.Any())
      {
        // Need to create sample data
        var filepath = Path.Combine(webHost.ContentRootPath, "Data/art.json");
        var json = File.ReadAllText(filepath);
        var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
        ctx.Products.AddRange(products);

        var order = ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
        if(order!= null)
        {
          order.User = user;
          order.Items = new List<OrderItem>()
          {
            new OrderItem()
            {
              Product = products.First(),
              Quantity = 5,
              UnitPrice = products.First().Price
            }
          };
        }

        ctx.SaveChanges();
      }
    }
  }
}
