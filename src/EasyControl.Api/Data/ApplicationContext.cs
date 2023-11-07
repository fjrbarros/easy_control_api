using EasyControl.Api.Data.Mappings;
using EasyControl.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyControl.Api.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(
                    e =>
                        e.Entity is BaseEntity
                        && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = new CancellationToken()
        )
        {
            var entries = ChangeTracker
                .Entries()
                .Where(
                    e =>
                        e.Entity is BaseEntity
                        && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
