using Microsoft.EntityFrameworkCore;

namespace OfferApp.Infrastructure.Database
{
    internal class EFDbInitializer : IDbInitializer
    {
        private readonly OfferDbContext _dbContext;

        public EFDbInitializer(OfferDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Start()
        {
            _dbContext.Database.Migrate();
        }
    }
}
