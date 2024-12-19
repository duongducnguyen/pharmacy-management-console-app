namespace PharmacyManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
