using System;
using System.Linq;
using System.Threading.Tasks;
using BaseProject.Catalog.Domain;
using BaseProject.Core.Data;
using BaseProject.Core.Messages;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Catalog.Infra
{
    public class CatalogContext : DbContext, IUnitOfWork
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedAt").IsModified = false;
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                }
            }

            var status = await base.SaveChangesAsync() > 0;
            return status;
        }

        public bool IsDBHelthy()
        {
            try
            {
                var canConnect = this.Database.CanConnect();
                Console.WriteLine($"Can connect to database: {canConnect}");
                return canConnect;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to connect to Database. {ex.Message}");
                throw;
            }
        }
    }
}

