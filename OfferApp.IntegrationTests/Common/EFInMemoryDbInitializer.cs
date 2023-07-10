using OfferApp.Infrastructure.Database;

namespace OfferApp.IntegrationTests.Common
{
    internal sealed class EFInMemoryDbInitializer : IDbInitializer
    {
        private readonly OfferDbContext _todoDbContext;

        public EFInMemoryDbInitializer(OfferDbContext todoDbContext)
        {
            _todoDbContext = todoDbContext;
        }

        public async Task Start()
        {
            await _todoDbContext.Database.EnsureCreatedAsync();
        }
    }
}
