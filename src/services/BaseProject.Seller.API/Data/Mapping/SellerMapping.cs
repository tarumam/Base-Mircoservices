using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Seller.API.Data.Mapping
{
    public class SellerMapping : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.ToTable("Seller");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.DocRef)
              .HasColumnType("varchar(30)");

            builder.Property(c => c.DocType)
              .HasColumnType("varchar(30)");

            builder.Property(c => c.Name)
               .IsRequired()
               .HasColumnType("varchar(100)");

            builder.Property(c => c.Address)
                .HasColumnType("varchar(200)");

            builder.Property(c => c.City)
               .HasColumnType("varchar(200)");

            builder.Property(c => c.Details)
               .HasColumnType("varchar(2000)");

            builder.Property(c => c.Neighbohood)
               .HasColumnType("varchar(30)");

            builder.Property(c => c.Number)
               .HasColumnType("varchar(10)");

            builder.Property(c => c.Reference)
               .HasColumnType("varchar(150)");

            builder.Property(c => c.WorkingTime)
               .HasColumnType("varchar(100)");

            builder.Property(c => c.HasParking);

            builder.Property(c => c.HasDelivery);

            builder.Property(c => c.Image)
               .HasColumnType("varchar(200)");
        }
    }
}
