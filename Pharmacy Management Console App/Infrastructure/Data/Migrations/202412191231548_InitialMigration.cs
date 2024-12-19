namespace PharmacyManagement.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Description = c.String(unicode: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Medicines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Description = c.String(unicode: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                        Unit = c.String(unicode: false),
                        ExpiryDate = c.DateTime(nullable: false, precision: 0),
                        CategoryId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderId = c.Int(nullable: false),
                        MedicineId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicines", t => t.MedicineId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.MedicineId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false, precision: 0),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CustomerId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Address = c.String(maxLength: 255, storeType: "nvarchar"),
                        Phone = c.String(maxLength: 20, storeType: "nvarchar"),
                        Email = c.String(maxLength: 255, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseId = c.Int(nullable: false),
                        MedicineId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicines", t => t.MedicineId, cascadeDelete: true)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.PurchaseId)
                .Index(t => t.MedicineId);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseDate = c.DateTime(nullable: false, precision: 0),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SupplierId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Address = c.String(maxLength: 255, storeType: "nvarchar"),
                        Phone = c.String(maxLength: 20, storeType: "nvarchar"),
                        Email = c.String(maxLength: 255, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Username, unique: true)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purchases", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseDetails", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.PurchaseDetails", "MedicineId", "dbo.Medicines");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.OrderDetails", "MedicineId", "dbo.Medicines");
            DropForeignKey("dbo.Medicines", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.Purchases", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseDetails", new[] { "MedicineId" });
            DropIndex("dbo.PurchaseDetails", new[] { "PurchaseId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.OrderDetails", new[] { "MedicineId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Medicines", new[] { "CategoryId" });
            DropTable("dbo.Users");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Purchases");
            DropTable("dbo.PurchaseDetails");
            DropTable("dbo.Customers");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Medicines");
            DropTable("dbo.Categories");
        }
    }
}
