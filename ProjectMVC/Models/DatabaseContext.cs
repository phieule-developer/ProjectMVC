namespace ProjectMVC.Models
{
     using System;
     using System.Data.Entity;
     using System.ComponentModel.DataAnnotations.Schema;
     using System.Linq;

     public partial class DatabaseContext : DbContext
     {
          public DatabaseContext()
              : base("name=DatabaseContext")
          {
          }

          public virtual DbSet<Admin> Admins { get; set; }
          public virtual DbSet<Category> Categories { get; set; }
          public virtual DbSet<Color> Colors { get; set; }
          public virtual DbSet<Comment> Comments { get; set; }
          public virtual DbSet<Description> Descriptions { get; set; }
          public virtual DbSet<Detail_Per> Detail_Per { get; set; }
          public virtual DbSet<Detail_Product> Detail_Product { get; set; }
          public virtual DbSet<District> Districts { get; set; }
          public virtual DbSet<Image> Images { get; set; }
          public virtual DbSet<Manufacturer> Manufacturers { get; set; }
          public virtual DbSet<Member> Members { get; set; }
          public virtual DbSet<Order> Orders { get; set; }
          public virtual DbSet<Order_Detail> Order_Detail { get; set; }
          public virtual DbSet<Permission> Permissions { get; set; }
          public virtual DbSet<Product> Products { get; set; }
          public virtual DbSet<Province> Provinces { get; set; }
          public virtual DbSet<SizeProduct> SizeProducts { get; set; }
          public virtual DbSet<Ward> Wards { get; set; }

          protected override void OnModelCreating(DbModelBuilder modelBuilder)
          {
               modelBuilder.Entity<Color>()
                   .HasMany(e => e.Detail_Product)
                   .WithOptional(e => e.Color)
                   .HasForeignKey(e => e.Color_Name);

               modelBuilder.Entity<Order_Detail>()
                   .Property(e => e.Price)
                   .HasPrecision(18, 0);

               modelBuilder.Entity<Product>()
                   .Property(e => e.Current_Price)
                   .HasPrecision(18, 0);

               modelBuilder.Entity<Product>()
                   .HasMany(e => e.Order_Detail)
                   .WithOptional(e => e.Product)
                   .HasForeignKey(e => e.Id);

               modelBuilder.Entity<SizeProduct>()
                   .HasMany(e => e.Detail_Product)
                   .WithOptional(e => e.SizeProduct)
                   .HasForeignKey(e => e.Size_Product);
          }
     }
}
