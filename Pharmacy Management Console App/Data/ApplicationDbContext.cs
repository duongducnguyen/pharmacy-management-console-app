using MySql.Data.EntityFramework;
using System.Data.Entity;
using PharmacyManagement.Models;

namespace PharmacyManagement.Data
{
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=DefaultConnection")
        {
            // Thay đổi này
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}