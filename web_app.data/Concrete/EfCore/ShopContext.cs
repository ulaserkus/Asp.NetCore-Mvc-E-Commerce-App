using System;
using Microsoft.EntityFrameworkCore;
using web_app.entity;

namespace web_app.data.Concrete.EfCore
{
    public class ShopContext : DbContext
    {

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        
        public DbSet<Order> Order { get; set; }

        public DbSet<CartItem> OrderItems { get; set; }
        


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ShopDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(c => new { c.CategoryId, c.ProductId });

        }
    }
}
