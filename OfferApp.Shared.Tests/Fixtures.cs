using OfferApp.Domain.Entities;
using OfferApp.Shared.DTO;

namespace OfferApp.Shared.Tests
{
    public static class Fixtures
    {
        public static Bid CreateBid()
        {
            return Bid.Create("Name#1", "Description12345", 100);
        }

        public static Bid CreatePublishedBid()
        {
            var bid = CreateBid();
            bid.Publish();
            return bid;
        }
    }
}