using Ecommerce.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<LocalUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OrderDetails>()
                .HasKey(e => new {e.Id, e.ProductId, e.OrderId});

            //// Data
            //modelBuilder.Entity<Category>().HasData(
            //   new Category { Id = 1, Name = "Electronics", Description = "Devices and gadgets" },
            //   new Category { Id = 2, Name = "Books", Description = "Books and literature" },
            //   new Category { Id = 3, Name = "Clothing", Description = "Apparel and accessories" }
            //);

            //modelBuilder.Entity<LocalUser>().HasData(
            //    new LocalUser { Id = "1", UserName = "Ahmed Haggag", Email = "Ahmad@gmail.com", PhoneNumber = "0568752124" },
            //    new LocalUser { Id = "2", UserName = "Tarek Sharim", Email = "Tarek@gmail.com", PhoneNumber = "0598752324" }
            //);

            //modelBuilder.Entity<Product>().HasData(
            //    new Product { Id = 1, Name = "Smartphone", Price = 299.99m, Image = "smartphone.jpg", CategoryId = 1 },
            //    new Product { Id = 2, Name = "Laptop", Price = 799.99m, Image = "laptop.jpg", CategoryId = 1 },
            //    new Product { Id = 3, Name = "Novel", Price = 19.99m, Image = "novel.jpg", CategoryId = 2 },
            //    new Product { Id = 4, Name = "T-Shirt", Price = 9.99m, Image = "tshirt.jpg", CategoryId = 3 },
            //    new Product { Id = 5, Name = "Jeans", Price = 49.99m, Image = "jeans.jpg", CategoryId = 3 }
            //);

            //modelBuilder.Entity<Order>().HasData(
            //    new Order { Id = 1, OrderStatus = "Pending", OrderDate = new DateTime(2023, 12, 11), LocalUserId = 1 },
            //    new Order { Id = 2, OrderStatus = "Completed", OrderDate = new DateTime(2023, 12, 12), LocalUserId = 2 },
            //    new Order { Id = 3, OrderStatus = "Shipped", OrderDate = new DateTime(2023, 12, 13), LocalUserId = 1 }
            //);

            //modelBuilder.Entity<OrderDetails>().HasData(
            //    new OrderDetails { Id = 1, OrderId = 1, ProductId = 1, Price = 299.99m, Quantity = 1 },
            //    new OrderDetails { Id = 2, OrderId = 1, ProductId = 4, Price = 9.99m, Quantity = 2 },
            //    new OrderDetails { Id = 3, OrderId = 2, ProductId = 3, Price = 19.99m, Quantity = 1 },
            //    new OrderDetails { Id = 4, OrderId = 3, ProductId = 2, Price = 799.99m, Quantity = 1 },
            //    new OrderDetails { Id = 5, OrderId = 3, ProductId = 5, Price = 9.99m, Quantity = 1 }
            //);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }

        //public DbSet<IdentityUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }
    }
}
