using BaseProject.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Catalog.Infra.Data.Mappings
{
    public class PriceMapping : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.ToTable("Price");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ProductId)
                .IsRequired();

            builder.Property(c => c.SellerId)
                .IsRequired();

            builder.Property(c => c.Value)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(c => c.Active)
                .IsRequired();

            builder.HasOne(p => p.Product)
                .WithMany(p => p.Prices)
                .HasForeignKey(p => p.ProductId);
        }
    }
}