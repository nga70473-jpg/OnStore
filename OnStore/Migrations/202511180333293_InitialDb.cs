namespace OnStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartItem",
                c => new
                    {
                        CartItemId = c.Int(nullable: false, identity: true),
                        CartId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CartItemId)
                .ForeignKey("dbo.Cart", t => t.CartId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.CartId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Cart",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 255),
                        Email = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 255),
                        Phone = c.String(maxLength: 20),
                        Address = c.String(),
                        Role = c.String(nullable: false, maxLength: 20),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CustomerName = c.String(nullable: false, maxLength: 100),
                        CustomerAddress = c.String(nullable: false, maxLength: 255),
                        CustomerEmail = c.String(nullable: false, maxLength: 100),
                        CustomerPhone = c.String(nullable: false, maxLength: 20),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        OrderItemId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderItemId)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        ProductName = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 500),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.ProductReview",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 1000),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProductSpec",
                c => new
                    {
                        SpecId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Attribute = c.String(nullable: false, maxLength: 255),
                        Value = c.String(nullable: false, maxLength: 600),
                    })
                .PrimaryKey(t => t.SpecId)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductTag",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.TagId })
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        TagName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.ProductVariant",
                c => new
                    {
                        VariantId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Color = c.String(nullable: false, maxLength: 100),
                        Storage = c.String(maxLength: 100),
                        RAM = c.String(maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ImageUrl = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.VariantId)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.PaymentMethod",
                c => new
                    {
                        PaymentMethodId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Method = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        TransactionId = c.String(nullable: false, maxLength: 100),
                        PaymentDate = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentMethodId)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "UserId", "dbo.Users");
            DropForeignKey("dbo.PaymentMethod", "OrderId", "dbo.Order");
            DropForeignKey("dbo.ProductVariant", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.ProductTag", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductSpec", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductReview", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProductReview", "ProductId", "dbo.Product");
            DropForeignKey("dbo.OrderItem", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.CartItem", "ProductId", "dbo.Product");
            DropForeignKey("dbo.OrderItem", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Cart", "UserId", "dbo.Users");
            DropForeignKey("dbo.CartItem", "CartId", "dbo.Cart");
            DropIndex("dbo.PaymentMethod", new[] { "OrderId" });
            DropIndex("dbo.ProductVariant", new[] { "ProductId" });
            DropIndex("dbo.ProductTag", new[] { "TagId" });
            DropIndex("dbo.ProductTag", new[] { "ProductId" });
            DropIndex("dbo.ProductSpec", new[] { "ProductId" });
            DropIndex("dbo.ProductReview", new[] { "UserId" });
            DropIndex("dbo.ProductReview", new[] { "ProductId" });
            DropIndex("dbo.Product", new[] { "CategoryId" });
            DropIndex("dbo.OrderItem", new[] { "ProductId" });
            DropIndex("dbo.OrderItem", new[] { "OrderId" });
            DropIndex("dbo.Order", new[] { "UserId" });
            DropIndex("dbo.Cart", new[] { "UserId" });
            DropIndex("dbo.CartItem", new[] { "ProductId" });
            DropIndex("dbo.CartItem", new[] { "CartId" });
            DropTable("dbo.PaymentMethod");
            DropTable("dbo.ProductVariant");
            DropTable("dbo.Tag");
            DropTable("dbo.ProductTag");
            DropTable("dbo.ProductSpec");
            DropTable("dbo.ProductReview");
            DropTable("dbo.Category");
            DropTable("dbo.Product");
            DropTable("dbo.OrderItem");
            DropTable("dbo.Order");
            DropTable("dbo.Users");
            DropTable("dbo.Cart");
            DropTable("dbo.CartItem");
        }
    }
}
