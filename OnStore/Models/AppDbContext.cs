using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnStore.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=AppDbContext")
        {
            // ✅ Tắt database initializer
            Database.SetInitializer<AppDbContext>(null);

            // ✅ Tắt lazy loading và proxy
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Users> Users { get; set; } //-1
        public DbSet<Category> Categories { get; set; } //-2
        public DbSet<Product> Products { get; set; } //-3
        public DbSet<ProductVariant> ProductVariants { get; set; } //-4
        public DbSet<ProductSpec> ProductSpecs { get; set; } //-5 
        public DbSet<ProductTag> ProductTags { get; set; } //-6
        public DbSet<ProductReview> productReviews { get; set; } //-7
        public DbSet<Tag> Tags { get; set; } //-8
        public DbSet<Cart> Carts { get; set; } //-9
        public DbSet<CartItem> CartItems { get; set; } //-10
        public DbSet<Order> Orders { get; set; } //-11
        public DbSet<OrderItem> OrderItems { get; set; } //-12
        public DbSet<PaymentMethod> PaymentMethods { get; set; }  //-13
        public object Brands { get; internal set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Cấu hình decimal cho TẤT CẢ properties kiểu decimal
            modelBuilder.Properties<decimal>()
                .Configure(c => c.HasPrecision(18, 2));

            // Cấu hình quan hệ 1-1
            modelBuilder.Entity<Users>()
                .HasOptional(u => u.Cart)
                .WithRequired(c => c.User);
        }
    }
}