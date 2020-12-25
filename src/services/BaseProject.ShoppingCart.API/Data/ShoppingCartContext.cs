using System.Linq;
using BaseProject.ShoppingCart.API.Model;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.ShoppingCart.API.Data
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<ShoppingCartClient> ShoppingCartClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model
                                        .GetEntityTypes()
                                        .SelectMany(e => e.GetProperties()
                                        .Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)");
            }

            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.Entity<ShoppingCartClient>()
                .HasIndex(c => c.ClientId)
                .HasName("IDX_CLIENT");

            modelBuilder.Entity<ShoppingCartClient>()
                .HasMany(c => c.Items)
                .WithOne(i => i.ShoppingCartClient)
                .HasForeignKey(c => c.ShoppingCartId);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        }
    }
}
