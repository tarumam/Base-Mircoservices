using BaseProject.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Catalog.Infra.Data.Mappings
{
    public class BluesoftTokenMapping : IEntityTypeConfiguration<BluesoftToken>
    {
        public void Configure(EntityTypeBuilder<BluesoftToken> builder)
        {
            builder.ToTable("BlueSoftToken");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Token)
                .IsRequired();

            builder.Property(c => c.Executions)
                .IsRequired();
        }
    }
}
