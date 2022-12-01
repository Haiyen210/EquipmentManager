using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using static NuGet.Packaging.PackagingConstants;

namespace EquipmentManager.Models.BusinessModels
{
    public class MyDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public MyDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
            }
        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Banner> Banners { get; set; } 
        public virtual DbSet<Category> Categories { get; set; } 
        public virtual DbSet<Favorite> Favorites { get; set; } 
        public virtual DbSet<Order> Orders { get; set; } 
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } 
        public virtual DbSet<Product> Products { get; set; } 
        public virtual DbSet<Rating> Ratings { get; set; } 
        public virtual DbSet<Role> Roles { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // khai báo thêm ràng buộc UNIQUE
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();
            // khai báo khóa chính trên nhiều trường cho bảng OrderDetail
            modelBuilder.Entity<OrderDetail>()
                 .HasKey(c => new { c.Id });
            //id Account tu tang
            modelBuilder.Entity<Account>(t =>
            {
                t.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            //id Banner tu tang
            modelBuilder.Entity<Banner>(t =>
            {
                t.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            //id Category tu tang
            modelBuilder.Entity<Category>(t =>
            {
                t.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            //id Customer tu tang
            modelBuilder.Entity<Account>(t =>
            {
                t.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            //Code Customer duy nhat
            modelBuilder.Entity<Account>().HasIndex(c => c.Code).IsUnique();

            //id Order tu tang
            modelBuilder.Entity<Order>(t =>
            {
                t.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            //id OrderDetail tu tang
            modelBuilder.Entity<OrderDetail>(t =>
            {
                t.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            //id Product tu tang
            modelBuilder.Entity<Product>(t =>
            {
                t.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Product>().HasKey(product => new { product.Id });



        }
    }
}
