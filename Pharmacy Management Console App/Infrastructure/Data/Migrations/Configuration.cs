namespace PharmacyManagement.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PharmacyManagement.Infrastructure.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Infrastructure\Data\Migrations";
        }

        protected override void Seed(PharmacyManagement.Infrastructure.Data.ApplicationDbContext context)
        {
            // Thêm dữ liệu mẫu
            context.Users.AddOrUpdate(
                u => u.Username,
                new PharmacyManagement.Core.Models.User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    Password = "123456", 
                    CreatedAt = System.DateTime.Now
                }
            );
        }
    }
}
