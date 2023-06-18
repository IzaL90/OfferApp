using OfferApp.Core.DTO;

namespace OfferApp.Core.Services
{
    public interface IBidService : IService
    {
        BidDto AddBid(BidDto dto);

        BidDto UpdateBid(BidDto dto);

        void DeleteBid(int id);

        BidDto? GetBidById(int id);

        IReadOnlyList<BidDto> GetAllBids();

        IReadOnlyList<BidPublishedDto> GetAllPublishedBids();

        bool Published(int id);
        
        bool Unpublished(int id);

        BidPublishedDto BidUp(int id, decimal price);
    }
}
