using CustomApproval.Web.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomApproval.Web.EntityFramework
{
    public class CustomApprovalDbContext : DbContext
    {
        public CustomApprovalDbContext(DbContextOptions<CustomApprovalDbContext> options)
              : base(options)
        {
        }

        public DbSet<UserData> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>(BuildUserDataModel);
        }

        private static void BuildUserDataModel(EntityTypeBuilder<UserData> entityTypeBuilder)
        {   
            entityTypeBuilder.ToTable("User");

            entityTypeBuilder.Property(user => user.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entityTypeBuilder.Property(user => user.Email)
                .IsRequired();
        }
    }
}
