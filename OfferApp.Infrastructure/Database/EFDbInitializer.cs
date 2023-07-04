using Microsoft.EntityFrameworkCore;

namespace OfferApp.Infrastructure.Database
{
    internal class EFDbInitializer : IDbInitializer
    {
        private readonly OfferDbContext _dbContext;
        private readonly DatabaseOptions _databaseOptions;

        public EFDbInitializer(OfferDbContext dbContext, DatabaseOptions databaseOptions)
        {
            _dbContext = dbContext;
            _databaseOptions = databaseOptions;
        }

        public void Start()
        {
            if (!_databaseOptions.RunMigrationsOnStart)
            {
                return;
            }

            _dbContext.Database.Migrate();
        }
    }
}
