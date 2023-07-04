using Microsoft.EntityFrameworkCore;
using OfferApp.Core.Entities;

namespace OfferApp.Infrastructure.Database
{
    public sealed class OfferDbContext : DbContext
    {
        public DbSet<Bid> Bids { get; set; }

        public OfferDbContext(DbContextOptions<OfferDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
