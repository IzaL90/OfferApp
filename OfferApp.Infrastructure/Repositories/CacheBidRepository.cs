using OfferApp.Core.Entities;
using OfferApp.Core.Repositories;
using OfferApp.Infrastructure.Cache;

namespace OfferApp.Infrastructure.Repositories
{
    internal sealed class CacheBidRepository : CacheRepository<Bid>, IBidRepository
    {
        private readonly IBidRepository _innerRepository;

        public CacheBidRepository(IBidRepository innerRepository, ICacheWrapper cacheWrapper)
            : base(innerRepository, cacheWrapper)
        {
            _innerRepository = innerRepository;
        }

        public Task<IReadOnlyList<Bid>> GetAllPublished()
        {
            return _innerRepository.GetAllPublished();
        }
    }
}
