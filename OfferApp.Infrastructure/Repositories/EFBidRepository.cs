using Microsoft.EntityFrameworkCore;
using OfferApp.Domain.Entities;
using OfferApp.Domain.Repositories;
using OfferApp.Infrastructure.Database;

namespace OfferApp.Infrastructure.Repositories
{
    internal sealed class EFBidRepository : IBidRepository
    {
        private readonly OfferDbContext _dbContext;

        public EFBidRepository(OfferDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Add(Bid entity)
        {
            _dbContext.Bids.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(Bid entity)
        {
            _dbContext.Bids.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public Task<Bid?> Get(int id)
        {
            return _dbContext.Bids.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IReadOnlyList<Bid>> GetAll()
        {
            return await _dbContext.Bids.AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<Bid>> GetAllPublished()
        {
            return await _dbContext.Bids
                .AsNoTracking()
                .Where(b => b.Published)
                .ToListAsync();
        }

        public async Task<bool> Update(Bid entity)
        {
            _dbContext.Bids.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
