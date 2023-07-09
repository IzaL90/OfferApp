using OfferApp.Core.Entities;
using OfferApp.Core.Repositories;

namespace OfferApp.Infrastructure.Repositories
{
    internal sealed class XmlBidRepository : XmlRepository<Bid>, IBidRepository
    {
        public async Task<IReadOnlyList<Bid>> GetAllPublished()
        {
            return (await GetAll()).Where(b => b.Published).ToList();
        }
    }
}
