using Microsoft.Extensions.Hosting;
using OfferApp.Domain.Entities;
using OfferApp.Infrastructure.Database;

namespace OfferApp.IntegrationTests.Common
{
    internal class SeedData : IHostedService
    {
        private readonly OfferDbContext _dbContext;

        public SeedData(OfferDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _dbContext.Bids.Add(Bid.Create("Bid#Name#1", "Bid#Description#1", 10));
            _dbContext.Bids.Add(Bid.Create("Bid#Name#2", "Bid#Description#2", 20));
            _dbContext.Bids.Add(Bid.Create("Bid#Name#3", "Bid#Description#3", 30));
            _dbContext.Bids.Add(Bid.Create("Bid#Name#4", "Bid#Description#4", 40));
            _dbContext.Bids.Add(Bid.Create("Bid#Name#5", "Bid#Description#5", 50));
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
