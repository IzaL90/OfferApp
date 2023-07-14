using OfferApp.Domain.Entities;

namespace OfferApp.Domain.Repositories
{
    public interface IBidRepository : IRepository<Bid>
    {
        Task<IReadOnlyList<Bid>> GetAllPublished();
    }
}
