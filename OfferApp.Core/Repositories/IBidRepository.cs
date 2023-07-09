using OfferApp.Core.Entities;

namespace OfferApp.Core.Repositories
{
    public interface IBidRepository : IRepository<Bid>
    {
        Task<IReadOnlyList<Bid>> GetAllPublished();
    }
}
