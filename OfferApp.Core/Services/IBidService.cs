using OfferApp.Core.DTO;

namespace OfferApp.Core.Services
{
    public interface IBidService : IService
    {
        Task<BidDto> AddBid(BidDto dto);

        Task<BidDto> UpdateBid(BidDto dto);

        Task DeleteBid(int id);

        Task<BidDto?> GetBidById(int id);

        Task<IReadOnlyList<BidDto>> GetAllBids();

        Task<IReadOnlyList<BidPublishedDto>> GetAllPublishedBids();

        Task<bool> Published(int id);
        
        Task<bool> Unpublished(int id);

        Task<BidPublishedDto> BidUp(BidUpDto bidUp);
    }
}
