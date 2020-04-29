using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
  public class DutchContext: IdentityDbContext<StoreUser>
  {
    public DutchContext(DbContextOptions<DutchContext> options): base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    //added this for orderItem
    //public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      //removed () after new order

      modelBuilder.Entity<Order>()                   
                  .HasData(new Order
                  {
                    Id = 1,
                    OrderDate = DateTime.UtcNow,
                    OrderNumber = "12345"
                  });
      //modelBuilder.Entity<OrderItem>()
      //            .Property(o => o.UnitPrice).HasColumnType("decimal(5,3");
      //modelBuilder.Entity<Product>()
      //            .Property(p => p.Price).HasColumnType("decimal(5,3");
    }

    

  }
}
