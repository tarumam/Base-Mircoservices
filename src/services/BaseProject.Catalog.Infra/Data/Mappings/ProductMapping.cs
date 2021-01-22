using BaseProject.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Catalog.Infra.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(c => c.Description)
                .IsRequired(false)
                .HasColumnType("varchar(2000)");

            builder.Property(c => c.Image)
                .IsRequired(false)
                .HasColumnType("varchar(1000)");

            builder.Property(c => c.Barcode)
                .HasColumnType("varchar(30)");

            builder.HasMany(p => p.Prices)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
