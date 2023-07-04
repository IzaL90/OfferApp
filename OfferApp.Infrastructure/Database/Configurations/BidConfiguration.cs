using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfferApp.Core.Entities;

namespace OfferApp.Infrastructure.Database.Configurations
{
    internal sealed class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .UseMySqlIdentityColumn();

            builder.Property(b => b.Name)
                .HasMaxLength(150);

            builder.Property(b => b.Description)
                .HasMaxLength(3000);

            builder.Property(b => b.FirstPrice)
                .HasPrecision(14,4)
                .IsRequired();

            builder.Property(b => b.LastPrice)
                .HasPrecision(14, 4);

            builder.Property(b => b.Count);

            builder.Property(b => b.Published);

            builder.Property(b => b.Created);
            builder.Property(b => b.Updated);

            builder.HasIndex(b => new { b.Name });
            builder.HasIndex(b => b.Updated);
        }
    }
}
