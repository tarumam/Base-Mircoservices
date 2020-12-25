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
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(c => c.Image)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(c => c.Barcode)
                .HasColumnType("varchar(15)");

            //builder.HasMany(p => p.Prices)
            //    .WithOne(p => p.Product)
            //    .HasForeignKey(p => p.ProductId);
        }
    }
}
